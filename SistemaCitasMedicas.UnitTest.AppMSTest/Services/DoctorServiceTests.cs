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
    public class DoctorServiceTests
    {
        private AppDBContext _context;
        private DoctorRepository _doctorRepository;
        private UsuarioRepository _usuarioRepository;
        private DoctorService _doctorService;
        private UsuarioService _usuarioService;

        [TestInitialize]
        public void Setup()
        {
            // Cargar configuración desde appsettings.Test.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)))
                .Options;

            _context = new AppDBContext(options);
            _doctorRepository = new DoctorRepository(_context);
            _usuarioRepository = new UsuarioRepository(_context);
            _doctorService = new DoctorService(_doctorRepository);
            _usuarioService = new UsuarioService(_usuarioRepository);
        }

        private async Task<Usuario> CrearUsuarioTemporalAsync()
        {
            var usuario = new Usuario
            {
                Nombre = "DoctorUser" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Correo = $"doctor{Guid.NewGuid().ToString("N").Substring(0, 5)}@test.com",
                PasswordHash = "pass123",
                IdRol = 1,
                Estado = 1
            };

            await _usuarioService.AgregarUsuarioAsync(usuario);
            var guardado = (await _usuarioRepository.GetUsuariosAsync())
                .FirstOrDefault(u => u.Correo == usuario.Correo);
            return guardado!;
        }

        [TestMethod]
        public async Task AgregarDoctorAsync_DeberiaInsertarCorrectamente()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var doctor = new Doctor
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Carlos",
                Apellido = "Ramírez",
                CedulaProfesional = "CED" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Telefono = "77777777",
                Horario = DateTime.Now.AddHours(1),
                Estado = 1
            };

            // Act
            var resultado = await _doctorService.AgregarDoctorAsync(doctor);

            // Assert
            Assert.AreEqual("Doctor agregado exitosamente.", resultado);

            var guardado = (await _doctorRepository.GetDoctoresAsync())
                .FirstOrDefault(d => d.IdUsuario == usuario.IdUsuario);

            Assert.IsNotNull(guardado);
            Assert.AreEqual(1, guardado.Estado);
        }

        [TestMethod]
        public async Task ObtenerTodosLosDoctoresAsync_DeberiaRetornarActivos()
        {
            // Act
            var doctores = await _doctorService.ObtenerTodosLosDoctoresAsync();

            // Assert
            Assert.IsNotNull(doctores);
            Assert.IsTrue(doctores.All(d => d.Estado == 1), "Se encontraron doctores inactivos en el resultado.");
        }

        [TestMethod]
        public async Task ObtenerDoctorPorIdAsync_DeberiaRetornarCorrectamente()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var doctor = new Doctor
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Mario",
                Apellido = "López",
                CedulaProfesional = "PROF" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Telefono = "12345678",
                Horario = DateTime.Now.AddHours(2),
                Estado = 1
            };
            doctor = await _doctorRepository.AddDoctorAsync(doctor);

            // Act
            var obtenido = await _doctorService.ObtenerDoctorPorIdAsync(doctor.IdDoctor);

            // Assert
            Assert.IsNotNull(obtenido);
            Assert.AreEqual(doctor.IdDoctor, obtenido.IdDoctor);
        }

        [TestMethod]
        public async Task ModificarDoctorAsync_DeberiaActualizarCorrectamente()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var doctor = new Doctor
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Ana",
                Apellido = "Martínez",
                CedulaProfesional = "DOC" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Telefono = "88888888",
                Horario = DateTime.Now.AddHours(3),
                Estado = 1
            };
            doctor = await _doctorRepository.AddDoctorAsync(doctor);

            doctor.Nombre = "Nombre Actualizado";
            doctor.Apellido = "Apellido Actualizado";

            // Act
            var resultado = await _doctorService.ModificarDoctorAsync(doctor);

            // Assert
            Assert.AreEqual("Doctor modificado exitosamente.", resultado);

            var actualizado = await _doctorRepository.GetDoctorByIdAsync(doctor.IdDoctor);
            Assert.AreEqual("Nombre Actualizado", actualizado.Nombre);
            Assert.AreEqual("Apellido Actualizado", actualizado.Apellido);
        }

        [TestMethod]
        public async Task DesactivarDoctorPorIdAsync_DeberiaCambiarEstadoACero()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var doctor = new Doctor
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Pedro",
                Apellido = "Hernández",
                CedulaProfesional = "DES" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Telefono = "99999999",
                Horario = DateTime.Now.AddHours(4),
                Estado = 1
            };
            doctor = await _doctorRepository.AddDoctorAsync(doctor);

            // Act
            var resultado = await _doctorService.DesactivarDoctorPorIdAsync(doctor.IdDoctor);

            // Assert
            Assert.AreEqual("Doctor desactivado exitosamente.", resultado);

            var actualizado = await _doctorRepository.GetDoctorByIdAsync(doctor.IdDoctor);
            Assert.AreEqual(0, actualizado.Estado);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context != null)
            {
                try
                {
                    // Eliminar doctores de prueba
                    var doctores = await _context.Doctores
                        .AsNoTracking()
                        .Where(d => d.CedulaProfesional.Contains("CED") ||
                                    d.CedulaProfesional.Contains("PROF") ||
                                    d.CedulaProfesional.Contains("DOC") ||
                                    d.CedulaProfesional.Contains("DES"))
                        .ToListAsync();

                    if (doctores.Any())
                    {
                        _context.Doctores.RemoveRange(doctores);
                        await _context.SaveChangesAsync();
                    }

                    // Eliminar usuarios de prueba
                    var usuarios = await _context.Usuarios
                        .AsNoTracking()
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
                    Console.WriteLine($"⚠ Error en Cleanup: {ex.Message}");
                }
            }
        }
    }
}
