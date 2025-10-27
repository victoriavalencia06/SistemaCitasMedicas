using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuario/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosActivosAsync();
            return Ok(usuarios);
        }

        // GET: api/usuario/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);

                if (usuario == null)
                    return NotFound($"No se encontró un usuario activo con ID {id}");

                return Ok(usuario);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/usuario/create
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Remove("Rol");
                return BadRequest(ModelState);
            }
            var resultado = await _usuarioService.AgregarUsuarioAsync(usuario);

            if (resultado.StartsWith("Error"))
                return BadRequest(resultado);

            return Ok(resultado);
        }

        // PUT: api/usuario/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                usuario.IdUsuario = id; // usa el id de la ruta

                var resultado = await _usuarioService.ModificarUsuarioAsync(usuario);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/usuario/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var resultado = await _usuarioService.DesactivarUsuarioPorIdAsync(id);
            return resultado.StartsWith("Error") ? NotFound(resultado) : Ok(resultado);
        }
    }
}
