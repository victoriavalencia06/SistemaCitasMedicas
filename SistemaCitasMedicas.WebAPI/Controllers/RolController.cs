using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [Authorize]
    [Route("api/rol")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolController(RolService rolService)
        {
            _rolService = rolService;
        }


        // GET: api/rol/get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> Get()
        {
            var roles = await _rolService.ObtenerRolesActivosAsync();
            return Ok(roles);
        }

        // GET api/<RolController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetById(int id)
        {
            try
            {
                var rol = await _rolService.ObtenerRolPorIdAsync(id);

                if (rol == null)
                    return NotFound($"No se encontró un rol activo con ID {id}");

                return Ok(rol);

            }
            catch (Exception ex)
            {
                // aquí ses podrrá registrar el error con ILogger
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST api/<RolController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _rolService.AgregarRolAsync(rol);

            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);

        }



        // PUT api/<RolController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Rol rol)
        {
            try
            {
                // El servicio valida si el id es válido o no coincide, no lo hacemos aquí
                rol.IdRol = id; // nos aseguramos de que use el id de la ruta

                var resultado = await _rolService.ModificarRolAsync(rol);

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

        // DELETE api/<RolController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _rolService.DesactivarRolPorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}