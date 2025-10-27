using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class RolServiceTests
    {
        private AppDBContext _context;
        private RolRepository _repository;
        private RolService _service;

        // Marcador único para identificar roles de prueba
        private readonly string _marker = "RolTest_";

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
            _repository = new RolRepository(_context);
            _service = new RolService(_repository);
        }

        [TestMethod]
        public async Task AgregarRolAsync_DeberiaInsertarCorrectamente()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            var rol = new Rol
            {
                Nombre = _marker + guid,
                Estado = 1
            };

            // Act
            var resultado = await _service.AgregarRolAsync(rol);

            // Assert
            // Nota: el servicio devuelve "Rol agregado correctamnete" (con la ortografía actual en el servicio)
            Assert.AreEqual("Rol agregado correctamente", resultado);

            var guardado = (await _repository.GetRolesAsync())
                .FirstOrDefault(r => r.Nombre == rol.Nombre);

            Assert.IsNotNull(guardado, "No se guardó el rol en la BD.");
            Assert.AreEqual(1, guardado.Estado);
        }

        [TestMethod]
        public async Task ObtenerRolesActivosAsync_DeberiaRetornarSoloActivos()
        {
            // Act
            var roles = await _service.ObtenerRolesActivosAsync();

            // Assert
            Assert.IsNotNull(roles);
            Assert.IsTrue(roles.All(r => r.Estado == 1), "Se encontraron roles inactivos en el resultado.");
        }

        [TestMethod]
        public async Task ObtenerRolPorIdAsync_DeberiaRetornarSoloSiActivo()
        {
            // Arrange: crear rol de prueba
            var guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            var rol = new Rol
            {
                Nombre = _marker + guid,
                Estado = 1
            };

            var agregado = await _repository.AddRolAsync(rol);

            // Act
            var obtenido = await _service.ObtenerRolPorIdAsync(agregado.IdRol);

            // Assert
            Assert.IsNotNull(obtenido, "No devolvió el rol activo existente.");
            Assert.AreEqual(agregado.IdRol, obtenido.IdRol);
        }

        [TestMethod]
        public async Task ModificarRolAsync_DeberiaActualizarCorrectamente()
        {
            // Arrange: crear rol de prueba
            var guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            var rol = new Rol
            {
                Nombre = _marker + guid,
                Estado = 1
            };

            var agregado = await _repository.AddRolAsync(rol);

            // Modificar
            agregado.Nombre = agregado.Nombre + "_Mod";
            agregado.Estado = 1;

            // Act
            var resultado = await _service.ModificarRolAsync(agregado);

            // Assert
            Assert.AreEqual("Rol modificado correctamente", resultado);

            var actualizado = await _repository.GetRolByIdAsync(agregado.IdRol);
            Assert.AreEqual(agregado.Nombre, actualizado.Nombre);
        }

        [TestMethod]
        public async Task AgregarRolAsync_CuandoNombreDuplicado_DeberiaRetornarError()
        {
            // Arrange: crear rol con nombre fijo
            var guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            var nombreDuplicado = _marker + guid;

            var rol1 = new Rol
            {
                Nombre = nombreDuplicado,
                Estado = 1
            };
            await _repository.AddRolAsync(rol1);

            var rol2 = new Rol
            {
                Nombre = nombreDuplicado,
                Estado = 1
            };

            // Act
            var resultado = await _service.AgregarRolAsync(rol2);

            // Assert
            Assert.AreEqual("Error: ya existe un rol con el mismo nombre", resultado);
        }

        [TestMethod]
        public async Task DesactivarRolPorIdAsync_DeberiaCambiarEstadoACero()
        {
            // Arrange: crear rol
            var guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            var rol = new Rol
            {
                Nombre = _marker + guid,
                Estado = 1
            };

            var agregado = await _repository.AddRolAsync(rol);

            // Act
            var resultado = await _service.DesactivarRolPorIdAsync(agregado.IdRol);

            // Assert
            Assert.AreEqual("Rol desactivado exitosamente.", resultado);

            var actualizado = await _repository.GetRolByIdAsync(agregado.IdRol);
            Assert.AreEqual(0, actualizado.Estado);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context == null) return;

            try
            {
                // Eliminar roles creados por pruebas (filtrando por marcador)
                var roles = await _context.Roles
                    .AsNoTracking()
                    .Where(r => r.Nombre.Contains(_marker))
                    .ToListAsync();

                if (roles.Any())
                {
                    _context.Roles.RemoveRange(roles);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error en Cleanup RolServiceTests: {ex.Message}");
            }
        }
    }
}
