using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Infrastructure.Data;
using SistemaCitasMedicas.Infrastructure.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.Application.Services.Tests
{
    [TestClass]
    public class CitaServiceTests
    {
        private AppDBContext _context;
        private CitaRepository _citaRepository;
        private CitaService _citaService;
        private UsuarioRepository _usuarioRepository;
        private UsuarioService _usuarioService;
        private PacienteRepository _pacienteRepository;
        private PacienteService _pacienteService;
        private DoctorRepository _doctorRepository;
        private DoctorService _doctorService;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)))
                .Options;

            _context = new AppDBContext(options);

            _citaRepository = new CitaRepository(_context);
            _citaService = new CitaService(_citaRepository);

            _usuarioRepository = new UsuarioRepository(_context);
            _usuarioService = new UsuarioService(_usuarioRepository);

            _pacienteRepository = new PacienteRepository(_context);
            _pacienteService = new PacienteService(_pacienteRepository);

            _doctorRepository = new DoctorRepository(_context);
            _doctorService = new DoctorService(_doctorRepository);
        }

        private async Task<Usuario> CrearUsuarioTemporalAsync(string prefijo)
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6);
            var usuario = new Usuario
            {
                Nombre = $"{prefijo}User{suffix}",
                Correo = $"{prefijo.ToLower()}{suffix}@test.com",
                PasswordHash = "testpass",
                IdRol = 1,
                Estado = 1
            };

            await _usuarioService.AgregarUsuarioAsync(usuario);
            var guardado = (await _usuarioRepository.GetUsuariosAsync())
                .First(u => u.Correo == usuario.Correo);
            return guardado;
        }

        private async Task<Paciente> CrearPacienteTemporalAsync()
        {
            var usuario = await CrearUsuarioTemporalAsync("Paciente");
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6);

            var paciente = new Paciente
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = $"PacienteTest{suffix}",
                Apellido = "Temporal",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Telefono = "70000000",
                Direccion = $"DirTemporalTest_{suffix}",
                Seguro = "SeguroTest",
                Estado = 1
            };

            await _pacienteService.AgregarPacienteAsync(paciente);

            var guardado = (await _pacienteRepository.GetPacientesAsync())
                .First(p => p.Direccion == paciente.Direccion);
            return guardado;
        }

        private async Task<Doctor> CrearDoctorTemporalAsync()
        {
            var usuario = await CrearUsuarioTemporalAsync("Doctor");
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6);

            var doctor = new Doctor
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = $"DoctorTest{suffix}",
                Apellido = "Temporal",
                CedulaProfesional = $"CED_TEST_{suffix}",
                Telefono = "71111111",
                Horario = DateTime.Now,
                Estado = 1
            };

            await _doctorService.AgregarDoctorAsync(doctor);

            var guardado = (await _doctorRepository.GetDoctoresAsync())
                .First(d => d.CedulaProfesional == doctor.CedulaProfesional);
            return guardado;
        }

        [TestMethod]
        public async Task AgregarCitaAsync_DeberiaInsertarCorrectamente()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var doctor = await CrearDoctorTemporalAsync();
            var usuario = (await _usuarioRepository.GetUsuariosAsync()).First(u => u.IdUsuario == paciente.IdUsuario);

            var fecha = DateTime.Now.AddMinutes(new Random().Next(60, 10000));

            var cita = new Cita
            {
                IdUsuario = usuario.IdUsuario,
                IdPaciente = paciente.IdPaciente,
                IdDoctor = doctor.IdDoctor,
                FechaHora = fecha,
                Estado = 1
            };

            var resultado = await _citaService.AgregarCitaAsync(cita);
            Assert.AreEqual("Cita agregada correctamente", resultado);

            var guardada = (await _citaRepository.GetCitasAsync())
                .FirstOrDefault(c => c.IdPaciente == paciente.IdPaciente && c.FechaHora == fecha);

            Assert.IsNotNull(guardada);
            Assert.AreEqual(1, guardada.Estado);
        }

        [TestMethod]
        public async Task ModificarCitaAsync_DeberiaActualizarCorrectamente()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var doctor = await CrearDoctorTemporalAsync();
            var usuario = (await _usuarioRepository.GetUsuariosAsync()).First(u => u.IdUsuario == paciente.IdUsuario);

            var fechaOriginal = DateTime.Now.AddMinutes(new Random().Next(10001, 20000));

            var cita = new Cita
            {
                IdUsuario = usuario.IdUsuario,
                IdPaciente = paciente.IdPaciente,
                IdDoctor = doctor.IdDoctor,
                FechaHora = fechaOriginal,
                Estado = 1
            };

            cita = await _citaRepository.AddCitaAsync(cita);

            var nuevaFecha = fechaOriginal.AddHours(1);
            cita.FechaHora = nuevaFecha;

            var resultado = await _citaService.ModificarCitaAsync(cita);
            Assert.AreEqual("Cita modificada correctamente", resultado);

            var actualizado = await _citaRepository.GetCitaByIdAsync(cita.IdCita);
            Assert.IsNotNull(actualizado);
            Assert.AreEqual(nuevaFecha, actualizado.FechaHora);
        }

        [TestMethod]
        public async Task DesactivarCitaPorIdAsync_DeberiaCambiarEstadoACero()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var doctor = await CrearDoctorTemporalAsync();
            var usuario = (await _usuarioRepository.GetUsuariosAsync()).First(u => u.IdUsuario == paciente.IdUsuario);

            var fecha = DateTime.Now.AddMinutes(new Random().Next(20001, 30000));

            var cita = new Cita
            {
                IdUsuario = usuario.IdUsuario,
                IdPaciente = paciente.IdPaciente,
                IdDoctor = doctor.IdDoctor,
                FechaHora = fecha,
                Estado = 1
            };

            cita = await _citaRepository.AddCitaAsync(cita);

            var resultado = await _citaService.DesactivarCitaPorIdAsync(cita.IdCita);
            Assert.AreEqual("Cita desactivado exitosamente.", resultado);

            var obtenido = await _citaService.ObtenerCitaPorIdAsync(cita.IdCita);
            Assert.IsNull(obtenido);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context == null) return;

            try
            {
                _context.ChangeTracker.Clear();

                var pacientesTemp = await _context.Pacientes
                    .Where(p => p.Direccion.Contains("DirTemporalTest_"))
                    .AsNoTracking()
                    .ToListAsync();

                if (pacientesTemp.Any())
                {
                    var pacienteIds = pacientesTemp.Select(p => p.IdPaciente).ToArray();
                    var citas = await _context.Citas
                        .Where(c => pacienteIds.Contains(c.IdPaciente))
                        .ToListAsync();

                    if (citas.Any())
                    {
                        _context.Citas.RemoveRange(citas);
                        await _context.SaveChangesAsync();
                    }

                    _context.Pacientes.RemoveRange(pacientesTemp);
                    await _context.SaveChangesAsync();
                }

                var doctoresTemp = await _context.Doctores
                    .Where(d => d.CedulaProfesional.Contains("CED_TEST_"))
                    .ToListAsync();

                if (doctoresTemp.Any())
                {
                    _context.Doctores.RemoveRange(doctoresTemp);
                    await _context.SaveChangesAsync();
                }

                var usuariosTemp = await _context.Usuarios
                    .Where(u => u.Correo.Contains("@test.com"))
                    .ToListAsync();

                if (usuariosTemp.Any())
                {
                    _context.Usuarios.RemoveRange(usuariosTemp);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error en Cleanup CitaServiceTests: {ex.Message}");
            }
        }
    }
}
