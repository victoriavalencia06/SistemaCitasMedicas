using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/historialmedico")]
    [Authorize]
    public class HistorialMedicoController : ControllerBase
    {
        private readonly HistorialMedicoServices _historialMedicoService;

        public HistorialMedicoController(HistorialMedicoServices historialMedicoService)
        {
            _historialMedicoService = historialMedicoService;
        }

        // GET: api/historialmedico/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<HistorialMedico>>> Get()
        {
            var historialesmedicos = await _historialMedicoService.ObtenerHistorialesMedicosActivosAsync();
            return Ok(historialesmedicos);
        }

        // GET: api/historialmedico/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<HistorialMedico>> GetById(int id)
        {
            try
            {
                var historialmedico = await _historialMedicoService.ObtenerHistorialMedicoPorIdAsync(id);

                if (historialmedico == null)
                    return NotFound($"No se encontró un historial médico con ID {id}");

                return Ok(historialmedico);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/historialmedico/create
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] HistorialMedico historialmedico)
        {
            ModelState.Remove("Paciente");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _historialMedicoService.AgregarHistorialMedicoAsync(historialmedico);

            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        // PUT: api/historialmedico/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] HistorialMedico historialmedico)
        {
            try
            {
                historialmedico.IdHistorialMedico = id; // usa el id de la ruta

                var resultado = await _historialMedicoService.ModificarHistorialMedicoAsync(historialmedico);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/historialmedico/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _historialMedicoService.DesactivarHistorialMedicoAsync(id);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
