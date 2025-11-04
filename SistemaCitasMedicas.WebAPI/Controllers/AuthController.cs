using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCitasMedicas.Application.DTOs;

namespace SistemaCitasMedicas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            var (ok, msg) = await _authService.RegisterAsync(usuario.Nombre, usuario.Correo, usuario.Password!, usuario.IdRol);
            if (!ok) return BadRequest(new { message = msg });
            return Ok(new { message = msg });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (ok, tokenOrMsg) = await _authService.LoginAsync(req.Correo, req.Password);
            if (!ok) return Unauthorized(new { message = tokenOrMsg });
            return Ok(new { token = tokenOrMsg });
        }
    }
}
