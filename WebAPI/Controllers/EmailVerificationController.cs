using Microsoft.AspNetCore.Mvc;
using CoreApp;
using DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        [HttpPost("verificar")]
        public async Task<IActionResult> VerificarCorreo([FromBody] VerificacionCorreoRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo))
            {
                return BadRequest("Correo es requerido.");
            }

            var usuario = await EmailVerificationManager.ValidarCorreoYEnviarOtp(request.Correo);

            if (usuario == null)
            {
                return NotFound("Correo no encontrado en nuestra base de datos.");
            }

            return Ok(new { mensaje = "OTP enviado con éxito." });
        }

        /////////////////////////////Linea nueva Validar codigos otp//////////////////////////////////////

        [HttpPost("verificar-otp")]
        public IActionResult VerificarCodigo([FromBody] VerificacionOtpRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Codigo))
            {
                return BadRequest("Correo y código son requeridos.");
            }

            var esValido = EmailVerificationManager.VerificarOtp(request.Correo, request.Codigo);

            if (!esValido)
            {
                return BadRequest(new { mensaje = "Código incorrecto o expirado." });
            }

            return Ok(new { mensaje = "Código verificado correctamente." });
        }

        public class VerificacionOtpRequest
        {
            public string Correo { get; set; }
            public string Codigo { get; set; }
        }

    }



    public class VerificacionCorreoRequest
    {
        public string Correo { get; set; }
    }
    
}
