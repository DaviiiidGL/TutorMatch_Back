using Microsoft.AspNetCore.Mvc;
using TutorMatch.Interfaces;
using TutorMatch.Models.DTOs;
using TutorMatch.Services;

namespace TutorMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ENDPOINT PARA REGISTRO
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Register(model.Email, model.Password, model.Role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = $"Usuario {model.Email} creado con éxito." });
        }

        // ENDPOINT PARA LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.Login(model.Email, model.Password);

            if (token is null)
                return Unauthorized(new { message = "Credenciales incorrectas." });

            return Ok(new { token });
        }
    }
}
