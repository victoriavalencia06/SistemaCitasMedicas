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
    [TestClass()]
    public class HistorialMedicoServicesTests
    {
        private AppDBContext _context;
        private HistorialMedicoRepository _historialRepository;
        private HistorialMedicoServices _historialService;
        private PacienteRepository _pacienteRepository;
        private PacienteService _pacienteService;
        private UsuarioRepository _usuarioRepository;
        private UsuarioService _usuarioService;

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
            _historialRepository = new HistorialMedicoRepository(_context);
            _historialService = new HistorialMedicoServices(_historialRepository);

            _usuarioRepository = new UsuarioRepository(_context);
            _usuarioService = new UsuarioService(_usuarioRepository);
            _pacienteRepository = new PacienteRepository(_context);
            _pacienteService = new PacienteService(_pacienteRepository);
        }

        private async Task<Paciente> CrearPacienteTemporalAsync()
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 5);

            var usuario = new Usuario
            {
                Nombre = $"HistUser{suffix}",
                Correo = $"hist{suffix}@test.com",
                PasswordHash = "pass123",
                IdRol = 1,
                Estado = 1
            };

            await _usuarioService.AgregarUsuarioAsync(usuario);
            var usuarioGuardado = (await _usuarioRepository.GetUsuariosAsync())
                .First(u => u.Correo == usuario.Correo);

            var paciente = new Paciente
            {
                IdUsuario = usuarioGuardado.IdUsuario,
                Nombre = $"Paciente{suffix}",
                Apellido = "Temporal",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Telefono = "55555555",
                Direccion = $"Dir Temporal {suffix}",
                Seguro = "SeguroTemporal",
                Estado = 1
            };

            await _pacienteService.AgregarPacienteAsync(paciente);
            var pacienteGuardado = (await _pacienteRepository.GetPacientesAsync())
                .First(p => p.Nombre == paciente.Nombre);

            return pacienteGuardado;
        }

        [TestMethod]
        public async Task AgregarHistorialMedicoAsync_DeberiaInsertarCorrectamente()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 5);

            var historial = new HistorialMedico
            {
                IdPaciente = paciente.IdPaciente,
                Diagnostico = $"DiagnosticoTest_{suffix}",
                Notas = "Notas prueba inserción",
                Tratamientos = "Tratamiento X",
                CuadroMedico = "Estable",
                FechaHora = DateTime.Now,
                Estado = 1
            };

            var resultado = await _historialService.AgregarHistorialMedicoAsync(historial);

            Assert.AreEqual("Historial médico agregado correctamente", resultado);

            var guardado = (await _historialRepository.GetHistorialesMedicosAsync())
                .FirstOrDefault(h => h.Diagnostico == historial.Diagnostico);

            Assert.IsNotNull(guardado);
            Assert.AreEqual(1, guardado.Estado);
        }

        [TestMethod]
        public async Task ObtenerHistorialMedicoPorIdAsync_DeberiaRetornarSoloActivo()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 5);

            var historial = new HistorialMedico
            {
                IdPaciente = paciente.IdPaciente,
                Diagnostico = $"DiagActivo_{suffix}",
                Notas = "Historial activo",
                Tratamientos = "Reposo",
                CuadroMedico = "Bueno",
                FechaHora = DateTime.Now,
                Estado = 1
            };
            historial = await _historialRepository.AddHistorialMedicoAsync(historial);

            var obtenido = await _historialService.ObtenerHistorialMedicoPorIdAsync(historial.IdHistorialMedico);

            Assert.IsNotNull(obtenido);
            Assert.AreEqual(historial.IdHistorialMedico, obtenido.IdHistorialMedico);
        }

        [TestMethod]
        public async Task ModificarHistorialMedicoAsync_DeberiaActualizarCorrectamente()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 5);

            var historial = new HistorialMedico
            {
                IdPaciente = paciente.IdPaciente,
                Diagnostico = $"DiagMod_{suffix}",
                Notas = "Inicio",
                Tratamientos = "Nada",
                CuadroMedico = "Regular",
                FechaHora = DateTime.Now,
                Estado = 1
            };
            historial = await _historialRepository.AddHistorialMedicoAsync(historial);

            historial.Notas = "Actualizado correctamente";
            historial.Tratamientos = "Nuevo tratamiento";
            var resultado = await _historialService.ModificarHistorialMedicoAsync(historial);

            Assert.AreEqual("Historial médico modificado correctamente", resultado);

            var actualizado = await _historialRepository.GetHistorialMedicoByIdAsync(historial.IdHistorialMedico);
            Assert.AreEqual("Actualizado correctamente", actualizado.Notas);
        }

        [TestMethod]
        public async Task DesactivarHistorialMedicoAsync_DeberiaCambiarEstadoACero()
        {
            var paciente = await CrearPacienteTemporalAsync();
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 5);

            var historial = new HistorialMedico
            {
                IdPaciente = paciente.IdPaciente,
                Diagnostico = $"DiagDes_{suffix}",
                Notas = "Activo",
                Tratamientos = "Tratamiento",
                CuadroMedico = "Estable",
                FechaHora = DateTime.Now,
                Estado = 1
            };
            historial = await _historialRepository.AddHistorialMedicoAsync(historial);

            var resultado = await _historialService.DesactivarHistorialMedicoAsync(historial.IdHistorialMedico);

            Assert.AreEqual("Historial médico desactivado exitosamente.", resultado);

            var actualizado = await _historialRepository.GetHistorialMedicoByIdAsync(historial.IdHistorialMedico);
            Assert.AreEqual(0, actualizado.Estado);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            try
            {
                _context.ChangeTracker.Clear(); // limpiar entidades en seguimiento

                // Historiales
                var historiales = await _context.HistorialesMedicos
                    .Where(h => h.Diagnostico.StartsWith("Diag") || h.Diagnostico.StartsWith("DiagnosticoTest_"))
                    .ToListAsync();
                if (historiales.Any())
                {
                    _context.HistorialesMedicos.RemoveRange(historiales);
                    await _context.SaveChangesAsync();
                }

                // Pacientes
                var pacientes = await _context.Pacientes
                    .Where(p => p.Direccion.Contains("Temporal"))
                    .ToListAsync();
                if (pacientes.Any())
                {
                    _context.Pacientes.RemoveRange(pacientes);
                    await _context.SaveChangesAsync();
                }

                // Usuarios
                var usuarios = await _context.Usuarios
                    .Where(u => u.Correo.Contains("@test.com"))
                    .ToListAsync();
                if (usuarios.Any())
                {
                    _context.Usuarios.RemoveRange(usuarios);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error en Cleanup HistorialMedicoServiceTests: {ex.Message}");
            }
        }
    }
}
