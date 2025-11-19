using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Application.DTOs;
using SistemaCitasMedicas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        // GET: api/rol
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolResponseDTO>>> GetAll()
        {
            try
            {
                var roles = await _rolService.GetAllRolesWithPermissionsAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/rol/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolResponseDTO>> GetById(int id)
        {
            try
            {
                var rol = await _rolService.GetRolWithPermissionsAsync(id);
                if (rol == null) return NotFound();
                return Ok(rol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/rol/menus
        [HttpGet("menus")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetAllMenus()
        {
            try
            {
                var menus = await _rolService.GetAllMenusAsync();
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/rol/1/menus
        [HttpGet("{idRol}/menus")]
        public async Task<ActionResult<IEnumerable<RolMenuDTO>>> GetMenusByRol(int idRol)
        {
            try
            {
                var menus = await _rolService.GetMenusPorRolAsync(idRol);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/rol
        [HttpPost]
        public async Task<ActionResult<RolResponseDTO>> Create([FromBody] RolCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var rolCreado = await _rolService.CrearRolConPermisosAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = rolCreado.IdRol }, rolCreado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/rol/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RolResponseDTO>> Update(int id, [FromBody] RolCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var rolActualizado = await _rolService.ActualizarRolConPermisosAsync(id, dto);
                return Ok(rolActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/rol/togglePermiso
        [HttpPut("togglePermiso")]
        public async Task<IActionResult> TogglePermiso([FromQuery] int idRol, [FromQuery] int idMenu, [FromQuery] bool habilitado)
        {
            try
            {
                var resultado = await _rolService.TogglePermisoAsync(idRol, idMenu, habilitado);
                if (!resultado)
                    return BadRequest("No se pudo actualizar el permiso");
                return Ok("Permiso actualizado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/rol/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _rolService.DesactivarRolAsync(id);
                if (!resultado) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/rol/5/reactivate
        [HttpPut("{id}/reactivate")]
        public async Task<ActionResult> Reactivate(int id)
        {
            try
            {
                var resultado = await _rolService.ReactivarRolAsync(id);
                if (!resultado) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ✅ ENDPOINTS LEGACY (para mantener compatibilidad con tu código existente)

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Rol>>> GetAllLegacy()
        {
            try
            {
                var roles = await _rolService.ObtenerRolesActivosAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("createWithPermissions")]
        public async Task<IActionResult> CreateWithPermissions([FromBody] RolCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _rolService.AgregarRolConPermisosAsync(dto);
            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPut("updateWithPermissions/{id}")]
        public async Task<IActionResult> UpdateWithPermissions(int id, [FromBody] RolCreateUpdateDTO dto)
        {
            try
            {
                var resultado = await _rolService.ActualizarRolConPermisosLegacyAsync(id, dto);
                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}