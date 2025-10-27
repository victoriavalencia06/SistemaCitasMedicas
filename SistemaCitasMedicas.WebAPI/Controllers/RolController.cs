using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/rol")]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolController(RolService rolService)
        {
            _rolService = rolService;
        }

        // GET: api/rol/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Rol>>> Get()
        {
            var roles = await _rolService.ObtenerRolesActivosAsync();
            return Ok(roles);
        }

        // GET: api/rol/get/5
        [HttpGet("get/{id}")]
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
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/rol/create
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _rolService.AgregarRolAsync(rol);

            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        // PUT: api/rol/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Rol rol)
        {
            try
            {
                rol.IdRol = id; // usa el id de la ruta

                var resultado = await _rolService.ModificarRolAsync(rol);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/rol/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _rolService.DesactivarRolPorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
