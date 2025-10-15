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
    public class DoctorEspecializacionController : ControllerBase
    {
        private readonly DoctorEspecializacionService _service;

        public DoctorEspecializacionController(DoctorEspecializacionService service) =>
            _service = service;

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<DoctorEspecializacion>>> GetEspecializacionesDeDoctor(int doctorId)
        {
            var lista = await _service.ObtenerEspecializacionesDeDoctorAsync(doctorId);
            return Ok(lista);
        }

        [HttpGet("especializacion/{especializacionId}")]
        public async Task<ActionResult<IEnumerable<DoctorEspecializacion>>> GetDoctoresPorEspecializacion(int especializacionId)
        {
            var lista = await _service.ObtenerDoctoresPorEspecializacionAsync(especializacionId);
            return Ok(lista);
        }

        [HttpPost("asignar")]
        public async Task<ActionResult<string>> Asignar([FromQuery] int doctorId, [FromQuery] int especializacionId)
        {
            var resultado = await _service.AsignarEspecializacionAsync(doctorId, especializacionId);
            return Ok(resultado);
        }

        [HttpDelete("quitar")]
        public async Task<ActionResult<string>> Quitar([FromQuery] int doctorId, [FromQuery] int especializacionId)
        {
            var resultado = await _service.QuitarEspecializacionAsync(doctorId, especializacionId);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
