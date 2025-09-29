using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
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
            var lista = await _service.ObtenerEspecializacionesDeDoctor(doctorId);
            return Ok(lista);
        }

        [HttpGet("especializacion/{especializacionId}")]
        public async Task<ActionResult<IEnumerable<DoctorEspecializacion>>> GetDoctoresPorEspecializacion(int especializacionId)
        {
            var lista = await _service.ObtenerDoctoresPorEspecializacion(especializacionId);
            return Ok(lista);
        }

        [HttpPost("asignar")]
        public async Task<ActionResult<string>> Asignar([FromQuery] int doctorId, [FromQuery] int especializacionId)
        {
            var resultado = await _service.AsignarEspecializacion(doctorId, especializacionId);
            return Ok(resultado);
        }

        [HttpDelete("quitar")]
        public async Task<ActionResult<string>> Quitar([FromQuery] int doctorId, [FromQuery] int especializacionId)
        {
            var resultado = await _service.QuitarEspecializacion(doctorId, especializacionId);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
