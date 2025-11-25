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
            var pacientes = await _pacienteService.ObtenerTodosPacientesAsync();
            return Ok(pacientes);
        }

        // GET: api/paciente/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Paciente>> GetById(int id)
        {
            var paciente = await _pacienteService.ObtenerPacientePorIdAsync(id);

            if (paciente == null)
                return NotFound($"No se encontró un paciente activo con el ID {id}.");

            return Ok(paciente);
        }

        // POST: api/paciente/create
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _pacienteService.AgregarPacienteAsync(paciente);

            return resultado.StartsWith("Error")
                ? BadRequest(resultado)
                : Ok(resultado);
        }

        // POST: api/paciente/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Paciente paciente)
        {
            paciente.IdPaciente = id;

            var resultado = await _pacienteService.ModificarPacienteAsync(paciente);

            return resultado.StartsWith("Error")
                ? BadRequest(resultado)
                : Ok(resultado);
        }

        // POST: api/paciente/delete
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _pacienteService.DesactivaPacientePorIdAsync(id);

            return resultado.StartsWith("Error")
                ? NotFound(resultado)
                : Ok(resultado);
        }
    }
}
