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
    public class PacienteServiceTests
    {
        private AppDBContext _context;
        private PacienteRepository _pacienteRepository;
        private PacienteService _pacienteService;
        private UsuarioRepository _usuarioRepository;
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
            _pacienteRepository = new PacienteRepository(_context);
            _usuarioRepository = new UsuarioRepository(_context);
            _usuarioService = new UsuarioService(_usuarioRepository);
            _pacienteService = new PacienteService(_pacienteRepository);
        }

        private async Task<Usuario> CrearUsuarioTemporalAsync()
        {
            var usuario = new Usuario
            {
                Nombre = "PacienteUser" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Correo = $"paciente{Guid.NewGuid().ToString("N").Substring(0, 5)}@test.com",
                PasswordHash = "pass123",
                IdRol = 1,
                Estado = 1
            };

            await _usuarioService.AgregarUsuarioAsync(usuario);
            var guardado = (await _usuarioRepository.GetUsuariosAsync())
                .FirstOrDefault(u => u.Correo == usuario.Correo);
            Assert.IsNotNull(guardado, "No se creó el usuario temporal para el paciente.");
            return guardado!;
        }

        [TestMethod]
        public async Task AgregarPacienteAsync_DeberiaInsertarCorrectamente()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var paciente = new Paciente
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Carlos",
                Apellido = "Mendoza",
                FechaNacimiento = new DateTime(1990, 5, 12),
                Telefono = "55555555",
                Direccion = "Av. Siempre Viva 123",
                Seguro = "SeguroTest",
                Estado = 1
            };

            // Act
            var resultado = await _pacienteService.AgregarPacienteAsync(paciente);

            // Assert
            Assert.AreEqual("Paciente agregado correctamente", resultado);

            var guardado = (await _pacienteRepository.GetPacientesAsync())
                .FirstOrDefault(p => p.Nombre == "Carlos" && p.Apellido == "Mendoza");
            Assert.IsNotNull(guardado);
            Assert.AreEqual(1, guardado.Estado);
        }

        [TestMethod]
        public async Task ObtenerPacientePorIdAsync_DeberiaRetornarSoloSiActivo()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var paciente = new Paciente
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Laura",
                Apellido = "Ramírez",
                FechaNacimiento = new DateTime(1995, 3, 10),
                Telefono = "12345678",
                Direccion = "Calle Falsa 456",
                Seguro = "SaludTotal",
                Estado = 1
            };
            paciente = await _pacienteRepository.AddPacienteAsync(paciente);

            // Act
            var obtenido = await _pacienteService.ObtenerPacientePorIdAsync(paciente.IdPaciente);

            // Assert
            Assert.IsNotNull(obtenido);
            Assert.AreEqual(paciente.IdPaciente, obtenido.IdPaciente);
        }

        [TestMethod]
        public async Task ModificarPacienteAsync_DeberiaActualizarCorrectamente()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var paciente = new Paciente
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Ana",
                Apellido = "Gómez",
                FechaNacimiento = new DateTime(1998, 11, 20),
                Telefono = "77777777",
                Direccion = "Calle Test 89",
                Seguro = "VidaPlena",
                Estado = 1
            };
            paciente = await _pacienteRepository.AddPacienteAsync(paciente);

            paciente.Nombre = "NombreActualizado";
            paciente.Apellido = "ApellidoActualizado";
            paciente.Direccion = "Nueva Dirección 123";

            // Act
            var resultado = await _pacienteService.ModificarPacienteAsync(paciente);

            // Assert
            Assert.AreEqual("Paciente modificado correctamente", resultado);

            var actualizado = await _pacienteRepository.GetPacienteByIdAsync(paciente.IdPaciente);
            Assert.AreEqual("NombreActualizado", actualizado.Nombre);
            Assert.AreEqual("ApellidoActualizado", actualizado.Apellido);
        }

        [TestMethod]
        public async Task DesactivaPacientePorIdAsync_DeberiaCambiarEstadoACero()
        {
            // Arrange
            var usuario = await CrearUsuarioTemporalAsync();

            var paciente = new Paciente
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = "Pedro",
                Apellido = "Hernández",
                FechaNacimiento = new DateTime(1992, 1, 1),
                Telefono = "99999999",
                Direccion = "Av. Central 101",
                Seguro = "SeguroInactivo",
                Estado = 1
            };
            paciente = await _pacienteRepository.AddPacienteAsync(paciente);

            // Act
            var resultado = await _pacienteService.DesactivaPacientePorIdAsync(paciente.IdPaciente);

            // Assert
            Assert.AreEqual("Paciente desactivado exitosamente.", resultado);

            var actualizado = await _pacienteRepository.GetPacienteByIdAsync(paciente.IdPaciente);
            Assert.AreEqual(0, actualizado.Estado);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context != null)
            {
                try
                {
                    // Eliminar pacientes de prueba
                    var pacientes = await _context.Pacientes
                        .AsNoTracking()
                        .Where(p => p.Direccion.Contains("Calle") || p.Direccion.Contains("Av."))
                        .ToListAsync();

                    if (pacientes.Any())
                    {
                        _context.Pacientes.RemoveRange(pacientes);
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
                    Console.WriteLine($"⚠ Error en Cleanup PacienteServiceTests: {ex.Message}");
                }
            }
        }
    }
}
