using _00_DTO;
using CoreApp;
using DTO;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly PagoManager manager;
        private readonly PayPalClient _paypalClient;

        public PagoController(PayPalClient paypalClient)
        {
            manager = new PagoManager();
            _paypalClient = paypalClient;
        }

        // POST: api/pago
        [HttpPost]
        public IActionResult RegistrarPago([FromBody] Pago pago)
        {
            try
            {
                manager.RegistrarPago(pago);
                return Ok("Pago registrado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el pago: {ex.Message}");
            }
        }

        // PUT: api/pago
        [HttpPut]
        public IActionResult ActualizarPago([FromBody] Pago pago)
        {
            try
            {
                manager.ActualizarPago(pago);
                return Ok("Pago actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el pago: {ex.Message}");
            }
        }

        // DELETE: api/pago/{id}
        [HttpDelete("{id}")]
        public IActionResult EliminarPago(int id)
        {
            try
            {
                manager.EliminarPago(id);
                return Ok("Pago eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el pago: {ex.Message}");
            }
        }

        // GET: api/pago
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            try
            {
                var pagos = manager.ObtenerTodos();
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los pagos: {ex.Message}");
            }
        }

        // GET: api/pago/{id}
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            try
            {
                var pago = manager.ObtenerPorId(id);
                if (pago == null)
                    return NotFound("Pago no encontrado.");

                return Ok(pago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el pago: {ex.Message}");
            }
        }

        // 📌 POST: api/pago/crear-orden-paypal?monto=50.00
        [HttpPost("crear-orden-paypal")]
        public async Task<IActionResult> CrearOrdenPayPal([FromQuery] double monto)
        {
            try
            {
                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(new OrderRequest
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            AmountWithBreakdown = new AmountWithBreakdown
                            {
                                CurrencyCode = "USD",
                                Value = monto.ToString("F2")
                            }
                        }
                    },
                    ApplicationContext = new ApplicationContext
                    {
                        ReturnUrl = "https://localhost:7131/api/pago/capturar-orden-paypal",
                        CancelUrl = "https://localhost:7131/api/pago/orden-cancelada"
                    }
                });

                var response = await _paypalClient.Client().Execute(request);
                var result = response.Result<Order>();

                var approveLink = result.Links.FirstOrDefault(l => l.Rel == "approve")?.Href;

                if (approveLink != null)
                    return Ok(new { urlAprobacion = approveLink });
                else
                    return BadRequest("No se pudo generar el link de aprobación de PayPal.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear orden PayPal: {ex.Message}");
            }
        }

        // 📌 GET: api/pago/capturar-orden-paypal?token=XXXX
        [HttpGet("capturar-orden-paypal")]
        public async Task<IActionResult> CapturarOrdenPayPal([FromQuery] string token)
        {
            try
            {
                var request = new OrdersCaptureRequest(token);
                request.Prefer("return=representation");

                var response = await _paypalClient.Client().Execute(request);
                var result = response.Result<Order>();

                // Si quieres registrar el pago en base de datos aquí
                // manager.RegistrarPago(new Pago { /* Info desde result */ });

                // Redirige al usuario a la página de resultado
                return Redirect($"/Pago/ResultadoPago?exito=true&mensaje=Pago%20completado%20correctamente.");
            }
            catch (Exception ex)
            {
                return Redirect($"/Pago/ResultadoPago?exito=false&mensaje={Uri.EscapeDataString(ex.Message)}");
            }
        }

        // 📌 GET: api/pago/orden-cancelada
        [HttpGet("orden-cancelada")]
        public IActionResult OrdenCancelada()
        {
            return Redirect("/Pago/ResultadoPago?exito=false&mensaje=Pago%20cancelado%20por%20el%20usuario.");
        }
    }
}
