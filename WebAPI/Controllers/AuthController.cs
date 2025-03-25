using Microsoft.AspNetCore.Mvc;
using CoreApp;  // Asegúrate de agregar esta referencia si la clase AuthManager está allí.
using DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Método POST para validar usuario
        [HttpPost("validar")]
        public IActionResult ValidarUsuario([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Contrasena))
            {
                return BadRequest("Correo y contraseña son requeridos.");
            }

            // Llamar a AuthManager para validar el usuario
            var usuario = AuthManager.ValidarUsuario(request.Correo, request.Contrasena);

            if (usuario == null)
            {
                return Unauthorized("Credenciales incorrectas o usuario no encontrado.");
            }

            // Si la validación es exitosa, retornar el usuario con un mensaje
            return Ok(new { mensaje = "Acceso exitoso", usuario = usuario });
        }
    }

    // Clase de solicitud que representa las credenciales del usuario
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}
