using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Application.Services;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: api/paciente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> Get()
        {
            try
            {
                var pacientes = await _pacienteService.ObtenerTodosPacientesAsync();
                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET api/paciente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetById(int id)
        {
            try
            {
                var paciente = await _pacienteService.ObtenerPacientePorIdAsync(id);
                if (paciente == null)
                    return NotFound($"No se encontró un paciente activo con ID {id}");

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST api/paciente
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            ModelState.Remove("Usuario");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _pacienteService.AgregarPacienteAsync(paciente);

            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        // PUT api/paciente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Paciente paciente)
        {
            try
            {
                paciente.IdPaciente = id; // Forzamos que coincida con el ID de la URL

                var resultado = await _pacienteService.ModificarPacienteAsync(paciente);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE api/paciente/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
