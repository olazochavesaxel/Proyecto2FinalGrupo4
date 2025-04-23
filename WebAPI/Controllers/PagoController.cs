using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using CoreApp;
using _00_DTO;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaypalController : ControllerBase
    {
        private readonly PagoManager _pagoManager;
        private readonly PayPalSettings _paypalSettings;

        public PaypalController(PagoManager pagoManager, IOptions<PayPalSettings> paypalSettings)
        {
            _pagoManager = pagoManager;
            _paypalSettings = paypalSettings.Value;
        }

        [HttpPost("create")]
        public IActionResult CreatePayment([FromBody] Pago pago)
        {
            try
            {
                _pagoManager.Create(pago);
                return Ok(new { message = "Pago registrado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear el pago", details = ex.Message });
            }
        }

        [HttpPost("crear-orden")]
        public async Task<IActionResult> CrearOrden([FromBody] MontoRequest request)
        {
            try
            {
                var token = await PayPalAuthService.ObtenerTokenAsync(_paypalSettings.ClientId, _paypalSettings.Secret);
                var json = await PayPalOrderService.CrearOrden(request.Monto, "USD", token);

                // Agregar aquí para ver el JSON completo recibido de PayPal
                Console.WriteLine("JSON completo recibido de PayPal: " + json);

                try
                {
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("id", out var idElement))
                        return StatusCode(500, new { error = "No se pudo crear la orden PayPal", details = json });

                    var orderId = idElement.GetString();
                    var link = root.GetProperty("links")
                        .EnumerateArray()
                        .First(l => l.GetProperty("rel").GetString() == "approve")
                        .GetProperty("href").GetString();

                    return Ok(new { orderId, link });
                }
                catch (JsonException ex)
                {
                    return StatusCode(500, new { error = "Error al procesar JSON de respuesta de PayPal", details = json });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error general al crear la orden en PayPal", details = ex.Message });
            }
        }

        [HttpPost("Confirmacion")]
        public async Task<IActionResult> CapturarOrden([FromBody] OrderIdRequest request)
        {
            try
            {
                var token = await PayPalAuthService.ObtenerTokenAsync(_paypalSettings.ClientId, _paypalSettings.Secret);
                var json = await PayPalOrderService.CapturarOrden(request.OrderId, token);

                var dto = PayPalMapper.Map(json);
                dto.PaypalOrderId = request.OrderId;

                if (string.IsNullOrEmpty(dto.PaymentCaptureId))
                {
                    throw new Exception("El ID de captura de PayPal no fue encontrado en la respuesta.");
                }

                // 🔽 Aquí agregas los Console.WriteLine para debug
                Console.WriteLine("Pago capturado:");
                Console.WriteLine($"OrderId: {dto.PaypalOrderId}");
                Console.WriteLine($"CaptureId: {dto.PaymentCaptureId}");
                Console.WriteLine($"Email: {dto.PayerEmail}");
                Console.WriteLine($"PayerId: {dto.PayerId}");
                Console.WriteLine($"Amount: {dto.Amount}");
                Console.WriteLine($"Currency: {dto.Currency}");
                Console.WriteLine($"Status: {dto.Status}");

                _pagoManager.Create(dto);

                return Ok(new { orderId = dto.PaypalOrderId, amount = dto.Amount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al capturar la orden de PayPal", details = ex.Message });
            }
        }


        [HttpPost("estado-orden")]
        public IActionResult EstadoOrden([FromBody] OrderIdRequest request)
        {
            try
            {
                var estado = new { status = "APPROVED" }; // Esto lo deberías reemplazar con tu lógica real
                return Ok(estado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el estado de la orden", details = ex.Message });
            }
        }
    }

    public class OrderIdRequest
    {
        public string OrderId { get; set; }
    }

    public class PayPalSettings
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }

        public PayPalSettings() { }

        public PayPalSettings(string clientId, string secret)
        {
            ClientId = clientId;
            Secret = secret;
        }
    }

    public class MontoRequest
    {
        public double Monto { get; set; }
    }
}
