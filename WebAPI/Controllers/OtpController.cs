using CoreApp;
using FinancialWebApp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        

        [HttpPost("validar")]
        public IActionResult ValidarOtp([FromBody] OtpValidacionRequest request)
        {
            bool esValido = OtpManager.ValidarOtp(request.Email, request.Otp);

            if (!esValido)
            {
                // Vuelve a intentar enviar OTP solo si el correo es válido y corresponde a un usuario
                try
                {
                    OtpManager.EnviarOtpSiUsuarioExiste(request.Email);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = $"Código inválido y error al reenviar OTP: {ex.Message}" });
                }

                return BadRequest(new { error = "El código es incorrecto o ha expirado. Se ha enviado uno nuevo." });
            }

            return Ok(new { message = "Código válido. Redirigiendo al inicio de sesión." });
        }
    }

    public class OtpValidacionRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
