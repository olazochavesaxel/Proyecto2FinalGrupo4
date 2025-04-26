using _00_DTO;
using CoreApp;
using DTO;
using DTOs;
using Microsoft.AspNetCore.Mvc;

using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreApp;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly TransaccionManager _transaccionManager;

        public TransaccionController()
        {
            _transaccionManager = new TransaccionManager();
        }

        // POST -> Guardar PayPal
        [HttpPost]
        [Route("GuardarPaypal")]
        public ActionResult GuardarPaypal([FromBody] Transaccion transaccion)
        {
            try
            {
                if (transaccion == null)
                {
                    return BadRequest(new { errors = new { general = new[] { "Los datos de la transacción son incorrectos." } } });
                }

                // Usar el método 'Create' como definiste originalmente
                _transaccionManager.Create(transaccion);

                return Ok(new { success = true, message = "Transacción guardada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { general = new[] { ex.Message } } });
            }
        }

    }
}
