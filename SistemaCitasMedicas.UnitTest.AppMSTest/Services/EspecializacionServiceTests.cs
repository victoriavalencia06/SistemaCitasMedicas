using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using SistemaCitasMedicas.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SistemaCitasMedicas.Application.Services.Tests
{
    [TestClass()]
    public class EspecializacionServiceTests
    {
        private AppDBContext _context;
        private EspecializacionRepository _repository;
        private EspecializacionService _service;

        [TestInitialize]
        public void Setup()
        {
            // Cargar la configuración desde appsettings.Test.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)))
                .Options;

            _context = new AppDBContext(options);
            _repository = new EspecializacionRepository(_context);
            _service = new EspecializacionService(_repository);
        }

        [TestMethod]
        public async Task AgregarEspecializacionAsync_DeberiaInsertarCorrectamente()
        {
            // Arrange
            var especializacion = new Especializacion
            {
                Nombre = "Cardiología2" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Descripcion = "Prueba de inserción",
                Estado = 1
            };

            // Act
            var resultado = await _service.AgregarEspecializacionAsync(especializacion);

            // Assert
            Assert.AreEqual("Especialización agregada exitosamente.", resultado);

            // Verificar que realmente se guardó en BD
            var guardada = (await _repository.GetEspecializacionesAsync())
                .FirstOrDefault(e => e.Nombre == especializacion.Nombre);

            Assert.IsNotNull(guardada);
            Assert.AreEqual(1, guardada.Estado);
        }

        [TestMethod]
        public async Task ObtenerTodasLasEspecializacionesAsync_DeberiaRetornarActivas()
        {
            // Act
            var lista = await _service.ObtenerTodasLasEspecializacionesAsync();

            // Assert
            Assert.IsNotNull(lista);
            Assert.IsTrue(lista.All(e => e.Estado == 1), "Hay especializaciones inactivas en el resultado");
        }

        [TestMethod]
        public async Task ObtenerEspecializacionPorIdAsync_DeberiaRetornarCorrectamente()
        {
            // Arrange
            var existente = (await _repository.GetEspecializacionesAsync()).FirstOrDefault();
            Assert.IsNotNull(existente, "No hay especializaciones en la BD para probar");

            // Act
            var resultado = await _service.ObtenerEspecializacionPorIdAsync(existente.IdEspecializacion);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(existente.IdEspecializacion, resultado.IdEspecializacion);
        }

        [TestMethod]
        public async Task ModificarEspecializacionAsync_DeberiaActualizarCorrectamente()
        {
            // Arrange
            var especializacion = (await _repository.GetEspecializacionesAsync()).FirstOrDefault();
            if (especializacion == null)
            {
                // Crear una nueva si no hay
                especializacion = new Especializacion
                {
                    Nombre = "Neurología2" + Guid.NewGuid().ToString("N").Substring(0, 5),
                    Descripcion = "Original",
                    Estado = 1
                };
                await _repository.AddEspecializacionAsync(especializacion);
            }

            especializacion.Descripcion = "Descripción actualizada";

            // Act
            var resultado = await _service.ModificarEspecializacionAsync(especializacion);

            // Assert
            Assert.AreEqual("Especialización modificada exitosamente.", resultado);

            // Verificar cambio real
            var actualizada = await _repository.GetEspecializacionByIdAsync(especializacion.IdEspecializacion);
            Assert.AreEqual("Descripción actualizada", actualizada.Descripcion);
        }

        [TestMethod]
        public async Task DesactivarEspecializacionByIdAsync_DeberiaCambiarEstadoACero()
        {
            // Arrange
            var especializacion = new Especializacion
            {
                Nombre = "Dermatología2" + Guid.NewGuid().ToString("N").Substring(0, 5),
                Descripcion = "Para desactivar",
                Estado = 1
            };
            especializacion = await _repository.AddEspecializacionAsync(especializacion);

            // Act
            var resultado = await _service.DesactivarEspecializacionByIdAsync(especializacion.IdEspecializacion);

            // Assert
            Assert.AreEqual("Especialización desactivada exitosamente.", resultado);

            // Verificar estado
            var actualizada = await _repository.GetEspecializacionByIdAsync(especializacion.IdEspecializacion);
            Assert.AreEqual(0, actualizada.Estado);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context != null && _repository != null)
            {
                try
                {
                    // Refresca la lista directamente desde la base
                    var items = await _context.Especializaciones
                        .AsNoTracking()
                        .Where(e => e.Nombre.Contains("ología") ||
                                    e.Nombre.Contains("Cardiología") ||
                                    e.Nombre.Contains("Dermatología"))
                        .ToListAsync();

                    if (items.Any())
                    {
                        _context.Especializaciones.RemoveRange(items);
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