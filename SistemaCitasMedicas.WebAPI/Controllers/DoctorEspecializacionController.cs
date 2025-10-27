using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/doctorEspecializacion")]
    [Authorize]
    public class DoctorEspecializacionController : ControllerBase
    {
        private readonly DoctorEspecializacionService _service;

        public DoctorEspecializacionController(DoctorEspecializacionService service) =>
            _service = service;

        // GET: api/doctorEspecializacion/getByDoctor/5
        [HttpGet("getByDoctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<DoctorEspecializacion>>> GetEspecializacionesDeDoctor(int doctorId)
        {
            var lista = await _service.ObtenerEspecializacionesDeDoctorAsync(doctorId);
            return Ok(lista);
        }

        // GET: api/doctorEspecializacion/getByEspecializacion/3
        [HttpGet("getByEspecializacion/{especializacionId}")]
        public async Task<ActionResult<IEnumerable<DoctorEspecializacion>>> GetDoctoresPorEspecializacion(int especializacionId)
        {
            var lista = await _service.ObtenerDoctoresPorEspecializacionAsync(especializacionId);
            return Ok(lista);
        }

        // POST: api/doctorEspecializacion/asignar
        [HttpPost("asignar")]
        public async Task<ActionResult<string>> Asignar([FromQuery] int doctorId, [FromQuery] int especializacionId)
        {
            var resultado = await _service.AsignarEspecializacionAsync(doctorId, especializacionId);
            return Ok(resultado);
        }

        // DELETE: api/doctorEspecializacion/quitar
        [HttpDelete("quitar")]
        public async Task<ActionResult<string>> Quitar([FromQuery] int doctorId, [FromQuery] int especializacionId)
        {
            var resultado = await _service.QuitarEspecializacionAsync(doctorId, especializacionId);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
