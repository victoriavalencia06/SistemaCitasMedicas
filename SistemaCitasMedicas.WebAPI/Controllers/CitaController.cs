using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Domain.Entities;
using SistemaCitasMedicas.Application.Services;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly CitaService _citaService;

        public CitaController(CitaService citaService)
        {
            _citaService = citaService;
        }

        // GET: api/cita
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> Get()
        {
            try
            {
                var citas = await _citaService.ObtenerTodasCitasAsync();
                return Ok(citas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET api/cita/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetById(int id)
        {
            try
            {
                var cita = await _citaService.ObtenerCitaPorIdAsync(id);
                if (cita == null)
                    return NotFound($"No se encontró una cita activa con ID {id}");

                return Ok(cita);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST api/cita
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("Paciente");
            ModelState.Remove("Doctor");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _citaService.AgregarCitaAsync(cita);

            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }


        // PUT api/cita/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cita cita)
        {
            try
            {
                cita.IdCita = id; // Asegura que el ID coincida con el de la URL

                var resultado = await _citaService.ModificarCitaAsync(cita);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE api/cita/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
