using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService) =>
            _doctorService = doctorService;

        // GET: api/doctor/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetTodos()
        {
            var doctores = await _doctorService.ObtenerTodosLosDoctoresAsync();
            return Ok(doctores);
        }

        // GET: api/doctor/get/5
        [HttpGet("get/{id}")]
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

        // POST: api/doctor/create
        [HttpPost("create")]
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

        // PUT: api/doctor/update/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.IdDoctor) return BadRequest("El ID no coincide.");

            var resultado = await _doctorService.ModificarDoctorAsync(doctor);
            return resultado.StartsWith("Error") ? BadRequest(resultado) : Ok(resultado);
        }

        // DELETE: api/doctor/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _doctorService.DesactivarDoctorPorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
