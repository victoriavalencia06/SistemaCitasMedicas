using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Authorize]
    [Route("api/historialmedico")]
    [ApiController]
    public class HistorialMedicoController : ControllerBase
    {
        private readonly HistorialMedicoServices _historialMedicoService;

        public HistorialMedicoController(HistorialMedicoServices historialMedicoService)
        {
            _historialMedicoService = historialMedicoService;
        }

        // GET: api/historialmedico/get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialMedico>>> Get()
        {
            var historialesmedicos = await _historialMedicoService.ObtenerHistorialesMedicosActivosAsync();
            return Ok(historialesmedicos);
        }

        // GET api/<HistorialMedicoController>/5
        [HttpGet("{id}")]
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
                // aquí se podrá registrar el error con ILogger
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST api/<HistorialMedicoController>
        [HttpPost]
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

        // PUT api/<HistorialMedicoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] HistorialMedico historialmedico)
        {
            try
            {
                // El servicio valida si el id es válido o no coincide, no lo hacemos aquí
                historialmedico.IdHistorialMedico = id; // nos aseguramos de que use el id de la ruta

                var resultado = await _historialMedicoService.ModificarHistorialMedicoAsync(historialmedico);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // 🔄 Registrar log aquí si tienes ILogger
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE api/historialmedico/5
        [HttpDelete("{id}")]
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
                // 🔄 Registrar log aquí si tienes ILogger
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
