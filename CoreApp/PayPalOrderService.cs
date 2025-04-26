using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PayPalOrderService
    {
        public static async Task<string> CrearOrden(double monto, string moneda, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
            new
            {
                amount = new
                {
                    currency_code = moneda,
                    value = monto.ToString("F2", CultureInfo.InvariantCulture)
                }
            }
        },
                application_context = new
                {
                    return_url = "http://localhost:5131/Confirmacion",
                    cancel_url = "http://localhost:5131/Cancelado"
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", content);
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<string> CapturarOrden(string orderId, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Este es el truco: un JSON vacío con content-type explícito
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var res = await client.PostAsync($"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture", content);
            return await res.Content.ReadAsStringAsync();
        }




    }
}
