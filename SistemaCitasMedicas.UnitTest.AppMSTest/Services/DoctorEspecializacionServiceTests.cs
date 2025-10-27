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
    public class DoctorEspecializacionServiceTests
    {
        private AppDBContext _context;
        private DoctorEspecializacionRepository _relRepo;
        private DoctorRepository _doctorRepo;
        private EspecializacionRepository _espRepo;
        private UsuarioRepository _usuarioRepo;

        private DoctorEspecializacionService _relService;
        private DoctorService _doctorService;
        private EspecializacionService _espService;
        private UsuarioService _usuarioService;

        // Prefijos/markers para facilitar limpieza
        private readonly string _markerCorreo = "@test.com";
        private readonly string _markerEspecializacion = "EspecializacionTest_";
        private readonly string _markerCedula = "CEDTEST_";

        [TestInitialize]
        public void Setup()
        {
            // Cargar configuración desde appsettings.Test.json (igual que en tus otros tests)
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)))
                .Options;

            _context = new AppDBContext(options);

            // Repositorios reales
            _relRepo = new DoctorEspecializacionRepository(_context);
            _doctorRepo = new DoctorRepository(_context);
            _espRepo = new EspecializacionRepository(_context);
            _usuarioRepo = new UsuarioRepository(_context);

            // Servicios reales
            _relService = new DoctorEspecializacionService(_relRepo);
            _doctorService = new DoctorService(_doctorRepo);
            _espService = new EspecializacionService(_espRepo);
            _usuarioService = new UsuarioService(_usuarioRepo);
        }

        /// <summary>
        /// Crea usuario -> doctor -> especializacion y devuelve sus ids.
        /// Usa marcadores únicos para facilitar limpieza posterior.
        /// </summary>
        private async Task<(int doctorId, int especializacionId, long usuarioId)> CrearDoctorYEspecializacionAsync()
        {
            // Crear usuario temporal
            var guidShort = Guid.NewGuid().ToString("N").Substring(0, 6);
            var usuario = new Usuario
            {
                Nombre = $"UsrDE_{guidShort}",
                Correo = $"usr_de_{guidShort}{_markerCorreo}",
                PasswordHash = "passTest123",
                IdRol = 1,
                Estado = 1
            };

            await _usuarioService.AgregarUsuarioAsync(usuario);

            // Recuperar usuario guardado (para obtener IdUsuario asignado)
            var usuarioGuardado = (await _usuarioRepo.GetUsuariosAsync())
                .FirstOrDefault(u => u.Correo == usuario.Correo);

            if (usuarioGuardado == null)
                throw new Exception("No se pudo crear el usuario de prueba.");

            // Crear doctor
            var doctor = new Doctor
            {
                IdUsuario = usuarioGuardado.IdUsuario,
                Nombre = "DrTest",
                Apellido = "Unit",
                CedulaProfesional = _markerCedula + guidShort,
                Telefono = "77777777",
                Horario = DateTime.Now.AddHours(1),
                Estado = 1
            };

            doctor = await _doctorRepo.AddDoctorAsync(doctor);

            // Crear especializacion con marcador único
            var especializacion = new Especializacion
            {
                Nombre = _markerEspecializacion + guidShort,
                Descripcion = "Creada para pruebas unitarias",
                Estado = 1
            };

            await _espService.AgregarEspecializacionAsync(especializacion);

            // Recuperar especializacion guardada (para obtener Id)
            var espGuardada = (await _espRepo.GetEspecializacionesAsync())
                .FirstOrDefault(e => e.Nombre == especializacion.Nombre);

            if (espGuardada == null)
                throw new Exception("No se pudo crear la especialización de prueba.");

            return (doctor.IdDoctor, espGuardada.IdEspecializacion, usuarioGuardado.IdUsuario);
        }

        [TestMethod]
        public async Task AsignarEspecializacionAsync_DeberiaAgregarRelacionCorrectamente()
        {
            // Arrange
            var (doctorId, especializacionId, _) = await CrearDoctorYEspecializacionAsync();

            // Act
            var mensaje = await _relService.AsignarEspecializacionAsync(doctorId, especializacionId);

            // Assert
            var relacion = (await _relRepo.GetEspecializacionesPorDoctorAsync(doctorId))
                                .FirstOrDefault(r => r.IdEspecializacion == especializacionId && r.IdDoctor == doctorId);

            Assert.IsNotNull(relacion, "No se encontró la relación creada en la BD.");
            Assert.AreEqual("Especialización asignada al doctor.", mensaje);
        }

        [TestMethod]
        public async Task ObtenerEspecializacionesDeDoctorAsync_DeberiaRetornarEspecializacionesCorrectamente()
        {
            // Arrange
            var (doctorId, especializacionId, _) = await CrearDoctorYEspecializacionAsync();
            await _relService.AsignarEspecializacionAsync(doctorId, especializacionId);

            // Act
            var resultado = await _relService.ObtenerEspecializacionesDeDoctorAsync(doctorId);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any(), "La lista de especializaciones del doctor está vacía.");
            Assert.IsTrue(resultado.Any(r => r.IdEspecializacion == especializacionId),
                "No se encontró la especialización esperada en el resultado.");
        }

        [TestMethod]
        public async Task ObtenerDoctoresPorEspecializacionAsync_DeberiaRetornarDoctoresCorrectamente()
        {
            // Arrange
            var (doctorId, especializacionId, _) = await CrearDoctorYEspecializacionAsync();
            await _relService.AsignarEspecializacionAsync(doctorId, especializacionId);

            // Act
            var resultado = await _relService.ObtenerDoctoresPorEspecializacionAsync(especializacionId);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any(), "La lista de doctores para la especialización está vacía.");
            Assert.IsTrue(resultado.Any(r => r.IdDoctor == doctorId),
                "No se encontró el doctor esperado en el resultado.");
        }

        [TestMethod]
        public async Task QuitarEspecializacionAsync_DeberiaEliminarRelacionCorrectamente()
        {
            // Arrange
            var (doctorId, especializacionId, _) = await CrearDoctorYEspecializacionAsync();
            await _relService.AsignarEspecializacionAsync(doctorId, especializacionId);

            // Act
            var mensaje = await _relService.QuitarEspecializacionAsync(doctorId, especializacionId);

            // Assert
            var existe = (await _relRepo.GetEspecializacionesPorDoctorAsync(doctorId))
                            .Any(r => r.IdEspecializacion == especializacionId && r.IdDoctor == doctorId);

            Assert.IsFalse(existe, "La relación no fue eliminada de la BD.");
            Assert.AreEqual("Especialización removida del doctor.", mensaje);
        }

        [TestMethod]
        public async Task QuitarEspecializacionAsync_DeberiaDevolverErrorSiNoExisteRelacion()
        {
            // Arrange
            var (doctorId, especializacionId, _) = await CrearDoctorYEspecializacionAsync();

            // Act
            var mensaje = await _relService.QuitarEspecializacionAsync(doctorId, especializacionId);

            // Assert
            Assert.AreEqual("Error: relación no encontrada.", mensaje);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_context == null) return;

            try
            {
                // 1) Eliminar relaciones creadas por pruebas (si existen)
                var relaciones = await _context.DoctorEspecializaciones
                    .AsNoTracking()
                    .Where(r => r.Doctor.CedulaProfesional != null && r.Doctor.CedulaProfesional.Contains(_markerCedula) ||
                                r.Especializacion.Nombre != null && r.Especializacion.Nombre.Contains(_markerEspecializacion))
                    .ToListAsync();

                if (relaciones.Any())
                {
                    _context.DoctorEspecializaciones.RemoveRange(relaciones);
                    await _context.SaveChangesAsync();
                }

                // 2) Eliminar doctores de prueba (por cedula)
                var doctores = await _context.Doctores
                    .AsNoTracking()
                    .Where(d => d.CedulaProfesional.Contains(_markerCedula))
                    .ToListAsync();

                if (doctores.Any())
                {
                    _context.Doctores.RemoveRange(doctores);
                    await _context.SaveChangesAsync();
                }

                // 3) Eliminar especializaciones de prueba (por nombre)
                var especializaciones = await _context.Especializaciones
                    .AsNoTracking()
                    .Where(e => e.Nombre.Contains(_markerEspecializacion))
                    .ToListAsync();

                if (especializaciones.Any())
                {
                    _context.Especializaciones.RemoveRange(especializaciones);
                    await _context.SaveChangesAsync();
                }

                // 4) Eliminar usuarios de prueba (por correo)
                var usuarios = await _context.Usuarios
                    .AsNoTracking()
                    .Where(u => u.Correo.Contains(_markerCorreo))
                    .ToListAsync();

                if (usuarios.Any())
                {
                    _context.Usuarios.RemoveRange(usuarios);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error en Cleanup DoctorEspecializacionServiceTests: {ex.Message}");
            }
        }
    }
}
