using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PayPalAuthService
    {
        public static async Task<string> ObtenerTokenAsync(string clientId, string secret)
        {
            using var client = new HttpClient();

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token", content);
            var json = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

    }
}
