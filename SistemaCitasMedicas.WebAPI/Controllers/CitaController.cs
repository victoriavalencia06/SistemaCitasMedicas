using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Authorize]
    [Route("api/cita")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly CitaService _citaService;

        public CitaController(CitaService citaService)
        {
            _citaService = citaService;
        }

        // DTO para devolver nombres en lugar de IDs
        public class CitaDTO
        {
            public int IdCita { get; set; }
            public string Paciente { get; set; }
            public string Doctor { get; set; }
            public DateTime FechaHora { get; set; }
            public bool Estado { get; set; }
        }

        // GET: api/cita/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<CitaDTO>>> Get()
        {
            try
            {
                var citas = await _citaService.ObtenerTodasCitasAsync();

                // Mapear citas a CitaDTO con nombre completo
                var citasDTO = citas.Select(c => new CitaDTO
                {
                    IdCita = c.IdCita,
                    Paciente = c.Paciente != null
                        ? (c.Paciente.Nombre ?? "") + " " + (c.Paciente.Apellido ?? "")
                        : "Sin paciente",
                    Doctor = c.Doctor != null
                        ? (c.Doctor.Nombre ?? "") + " " + (c.Doctor.Apellido ?? "")
                        : "Sin doctor",
                    FechaHora = c.FechaHora,
                    Estado = c.Estado == 1
                }).ToList();

                return Ok(citasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET api/cita/get/5
        [HttpGet("get/{id}")]
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

        // POST api/cita/create
        [HttpPost("create")]
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

        // PUT api/cita/update/5
        [HttpPut("update/{id}")]
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

        // DELETE api/cita/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _citaService.DesactivarCitaPorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
