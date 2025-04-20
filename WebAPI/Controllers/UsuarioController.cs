using CoreApp;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioManager _usuarioManager;

        public UsuarioController()
        {
            _usuarioManager = new UsuarioManager();
        }

        [HttpPost("actualizar-contrasena")]
        public IActionResult ActualizarContrasena([FromBody] CambiarContrasenaRequest request)
        {
            try
            {
                PasswordRecoveryManager.ActualizarContrasena(request.Email, request.NuevaContrasena);
                return Ok(new { message = "Contraseña actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error interno: {ex.Message}", stackTrace = ex.StackTrace });
            }

        }

        public class CambiarContrasenaRequest
        {
            public string Email { get; set; }
            public string NuevaContrasena { get; set; }
        }
    }
}
