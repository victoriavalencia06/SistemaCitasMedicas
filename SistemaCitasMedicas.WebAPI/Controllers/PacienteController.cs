using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/paciente")]
    [Authorize]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: api/paciente/getAll
        [HttpGet("getAll")]
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

        // GET: api/paciente/get/5
        [HttpGet("get/{id}")]
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

        // POST: api/paciente/create
        [HttpPost("create")]
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

        // PUT: api/paciente/update/5
        [HttpPut("update/{id}")]
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

        // DELETE: api/paciente/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _pacienteService.DesactivaPacientePorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
