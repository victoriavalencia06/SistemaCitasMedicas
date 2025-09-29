using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecializacionController : ControllerBase
    {
        private readonly EspecializacionService _especializacionService;

        public EspecializacionController(EspecializacionService especializacionService)
        {
            _especializacionService = especializacionService;
        }

        // GET: api/Especializacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especializacion>>> GetTodas()
        {
            var especializaciones = await _especializacionService.ObtenerTodasLasEspecializaciones();
            return Ok(especializaciones);
        }

        // GET: api/Especializacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Especializacion>> GetPorId(int id)
        {
            try
            {
                var especializacion = await _especializacionService.ObtenerEspecializacionPorId(id);
                return Ok(especializacion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }

        // POST: api/Especializacion
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Especializacion especializacion)
        {
            var resultado = await _especializacionService.AgregarEspecializacion(especializacion);
            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        // PUT: api/Especializacion/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromBody] Especializacion especializacion)
        {
            if (id != especializacion.IdEspecializacion)
                return BadRequest("El ID del parámetro no coincide con el ID de la especialización.");

            var resultado = await _especializacionService.ModificarEspecializacion(especializacion);
            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        // DELETE: api/Especializacion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                var especializacion = await _especializacionService.ObtenerEspecializacionPorId(id);
                especializacion.Estado = 0; // Baja lógica, lo marcamos como inactivo
                var resultado = await _especializacionService.ModificarEspecializacion(especializacion);

                return Ok(resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }
    }
}
