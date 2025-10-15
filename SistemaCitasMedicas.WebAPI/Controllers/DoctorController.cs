using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService) =>
            _doctorService = doctorService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetTodos()
        {
            var doctores = await _doctorService.ObtenerTodosLosDoctoresAsync();
            return Ok(doctores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetPorId(int id)
        {
            try
            {
                var doctor = await _doctorService.ObtenerDoctorPorIdAsync(id);
                return Ok(doctor);
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (System.Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid) 
            {
                ModelState.Remove("Usuario");
                return BadRequest(ModelState);
            }
            var resultado = await _doctorService.AgregarDoctorAsync(doctor);
            return resultado.StartsWith("Error") ? BadRequest(resultado) : Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.IdDoctor) return BadRequest("El ID no coincide.");

            var resultado = await _doctorService.ModificarDoctorAsync(doctor);
            return resultado.StartsWith("Error") ? BadRequest(resultado) : Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _doctorService.DesactivarDoctorPorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
