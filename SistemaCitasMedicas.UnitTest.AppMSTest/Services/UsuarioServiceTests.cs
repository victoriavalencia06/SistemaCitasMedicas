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
    public class UsuarioServiceTests
    {
        private AppDBContext _context;
        private UsuarioRepository _repository;
        private UsuarioService _service;

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
            _repository = new UsuarioRepository(_context);
            _service = new UsuarioService(_repository);
        }

        [TestMethod]
        public async Task AgregarUsuarioAsync_DeberiaInsertarCorrectamente()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Carlos" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Correo = $"carlos{Guid.NewGuid().ToString("N").Substring(0, 5)}@test.com",
                PasswordHash = "123456",
                IdRol = 1,
                Estado = 1
            };

            // Act
            var resultado = await _service.AgregarUsuarioAsync(usuario);

            // Assert
            Assert.AreEqual("Usuario agregado correctamente", resultado);

            var guardado = (await _repository.GetUsuariosAsync())
                .FirstOrDefault(u => u.Correo == usuario.Correo);

            Assert.IsNotNull(guardado);
            Assert.AreEqual(1, guardado.Estado);
        }

        [TestMethod]
        public async Task ObtenerUsuariosActivosAsync_DeberiaRetornarSoloActivos()
        {
            // Act
            var usuarios = await _service.ObtenerUsuariosActivosAsync();

            // Assert
            Assert.IsNotNull(usuarios);
            Assert.IsTrue(usuarios.All(u => u.Estado == 1), "Se encontraron usuarios inactivos en el resultado.");
        }

        [TestMethod]
        public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarSoloSiActivo()
        {
            // Arrange
            var usuarioExistente = (await _repository.GetUsuariosAsync()).FirstOrDefault();
            Assert.IsNotNull(usuarioExistente, "No hay usuarios en la BD para probar.");

            // Act
            var usuario = await _service.ObtenerUsuarioPorIdAsync(usuarioExistente.IdUsuario);

            // Assert
            if (usuarioExistente.Estado == 1)
                Assert.IsNotNull(usuario, "No devolvió usuario activo existente.");
            else
                Assert.IsNull(usuario, "Devolvió usuario inactivo.");
        }

        [TestMethod]
        public async Task ModificarUsuarioAsync_DeberiaActualizarCorrectamente()
        {
            // Arrange
            var usuario = (await _repository.GetUsuariosAsync()).FirstOrDefault();
            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = "Laura" + Guid.NewGuid().ToString("N").Substring(0, 5),
                    Correo = $"laura{Guid.NewGuid().ToString("N").Substring(0, 5)}@test.com",
                    PasswordHash = "abc123",
                    IdRol = 1,
                    Estado = 1
                };
                usuario = await _repository.AddUsuarioAsync(usuario);
            }

            usuario.Nombre = "Nombre Actualizado";

            // Act
            var resultado = await _service.ModificarUsuarioAsync(usuario);

            // Assert
            Assert.AreEqual("Usuario modificado correctamente", resultado);

            var actualizado = await _repository.GetUsuarioByIdAsync(usuario.IdUsuario);
            Assert.AreEqual("Nombre Actualizado", actualizado.Nombre);
        }

        [TestMethod]
        public async Task DesactivarUsuarioPorIdAsync_DeberiaCambiarEstadoACero()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Miguel" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Correo = $"miguel{Guid.NewGuid().ToString("N").Substring(0, 5)}@test.com",
                PasswordHash = "xyz123",
                IdRol = 1,
                Estado = 1
            };
            usuario = await _repository.AddUsuarioAsync(usuario);

            // Act
            var resultado = await _service.DesactivarUsuarioPorIdAsync((int)usuario.IdUsuario);

            // Assert
            Assert.AreEqual("Usuario desactivado exitosamente.", resultado);

            var actualizado = await _repository.GetUsuarioByIdAsync(usuario.IdUsuario);
            Assert.AreEqual(0, actualizado.Estado);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context != null && _repository != null)
            {
                try
                {
                    var items = await _context.Usuarios
                        .AsNoTracking()
                        .Where(u => u.Correo.Contains("@test.com"))
                        .ToListAsync();

                    if (items.Any())
                    {
                        _context.Usuarios.RemoveRange(items);
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
