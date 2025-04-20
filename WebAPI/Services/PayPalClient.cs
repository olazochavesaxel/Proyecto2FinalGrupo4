using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System.Threading.Tasks;
using System;

namespace WebAPI.Services
{
    public class PayPalClient
    {
        private readonly IConfiguration _config;
        private readonly PayPalEnvironment _environment;
        private readonly PayPalHttpClient _client;

        public PayPalClient(IConfiguration config)
        {
            _config = config;

            var clientId = _config["ATGiOHE73IgggH94DiJFpRAR5G4sr3DZymC6-50uX9jjg2vwReDa2zKk1FfK631i4Hhvd91UPAdo5Zri"];
            var secret = _config["EOAxgPdyxAwCCOh111JXt7EkNveH9acFngnYEZVH6wArTKfrHd6Xgj0-WikQVLahUfydSuB3Q9brXFVx"];

            _environment = new SandboxEnvironment(clientId, secret); // usa LiveEnvironment para producción
            _client = new PayPalHttpClient(_environment);
        }

        // Crear Orden de pago
        public async Task<string> CrearPagoAsync(double monto)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new System.Collections.Generic.List<PurchaseUnitRequest>
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
                    ReturnUrl = "https://tusitio.com/orden-aprobada",
                    CancelUrl = "https://tusitio.com/orden-cancelada"
                }
            });

            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            return result.Id; // devuelve el ID de la orden creada
        }

        // Capturar Orden aprobada
        public async Task<Order> CapturarPagoAsync(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            return result;
        }
    }
}
