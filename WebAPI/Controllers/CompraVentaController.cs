using CoreApp;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraVentaController : ControllerBase
    {
        private readonly CompraVentaManager _compraVentaManager;

        public CompraVentaController()
        {
            _compraVentaManager = new CompraVentaManager(); // Se recomienda usar inyección de dependencias en lugar de instanciar aquí.
        }

        // POST -> Registrar inversión
        [HttpPost]
        [Route("RegistrarInversion")]
        public async Task<ActionResult> RegistrarInversion([FromBody] RegistrarInversionRequest request)
        {
            try
            {
                // Llamar al método RegistrarInversion desde CompraVentaManager
                await _compraVentaManager.RegistrarInversion(request.IdCliente, request.IdAccion, request.Cantidad, request.Tipo);

                // Responder con éxito
                return Ok(new { success = true, message = "Inversión registrada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Error al registrar la inversión: " + ex.Message } } });
            }
        }
    }

    // DTO para RegistrarInversion (Se puede personalizar según sea necesario)
    public class RegistrarInversionRequest
    {
        public int IdCliente { get; set; }
        public int IdAccion { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; }
    }
}
