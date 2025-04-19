using _00_DTO;
using CoreApp;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private PagoManager manager = new PagoManager();

        [HttpPost]
        public IActionResult RegistrarPago([FromBody] Pago pago)
        {
            try
            {
                manager.RegistrarPago(pago);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
