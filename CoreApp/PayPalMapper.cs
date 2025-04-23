using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using _00_DTO;

namespace CoreApp
{

    public static class PayPalMapper
    {
        public static Pago Map(string json)
        {
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var pago = new Pago();

            if (root.TryGetProperty("id", out var id))
                pago.PaypalOrderId = id.GetString();
            else
                Console.WriteLine("Missing 'id' property in PayPal response.");

            if (root.TryGetProperty("payer", out var payer))
            {
                if (payer.TryGetProperty("email_address", out var email))
                    pago.PayerEmail = email.GetString();
                else
                    Console.WriteLine("Missing 'email_address' in payer.");

                if (payer.TryGetProperty("payer_id", out var payerId))
                    pago.PayerId = payerId.GetString();
                else
                    Console.WriteLine("Missing 'payer_id' in payer.");
            }

            if (root.TryGetProperty("status", out var status))
                pago.Status = status.GetString();
            else
                Console.WriteLine("Missing 'status' in PayPal response.");

            // Se eliminan las líneas de create_time y update_time, ya que se gestionan automáticamente en la base de datos

            if (root.TryGetProperty("purchase_units", out var units) && units.GetArrayLength() > 0)
            {
                var unit = units[0];
                if (unit.TryGetProperty("payments", out var payments))
                {
                    if (payments.TryGetProperty("captures", out var captures) && captures.GetArrayLength() > 0)
                    {
                        var capture = captures[0];

                        if (capture.TryGetProperty("id", out var captureId))
                            pago.PaymentCaptureId = captureId.GetString();
                        else
                            Console.WriteLine("Missing 'id' in capture.");

                        if (capture.TryGetProperty("amount", out var amount))
                        {
                            if (amount.TryGetProperty("value", out var val))
                                pago.Amount = double.Parse(val.GetString(), CultureInfo.InvariantCulture);
                            else
                                Console.WriteLine("Missing 'value' in amount.");

                            if (amount.TryGetProperty("currency_code", out var currency))
                                pago.Currency = currency.GetString();
                            else
                                Console.WriteLine("Missing 'currency_code' in amount.");
                        }
                        else
                            Console.WriteLine("Missing 'amount' in capture.");
                    }
                    else
                        Console.WriteLine("Missing 'captures' in payments.");
                }
                else
                    Console.WriteLine("Missing 'payments' in purchase_units.");
            }
            else
                Console.WriteLine("Missing 'purchase_units' or it's empty.");

            return pago;
        }



        public static void ValidarJsonCaptura(string json)
        {
            var errores = new List<string>();
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("id", out _))
                errores.Add("Falta el campo 'id' de la orden.");

            if (!root.TryGetProperty("status", out _))
                errores.Add("Falta el campo 'status'.");

            if (!root.TryGetProperty("purchase_units", out var units) || units.GetArrayLength() == 0)
                errores.Add("Falta 'purchase_units' o está vacío.");
            else
            {
                var unit = units[0];

                if (!unit.TryGetProperty("payments", out var payments))
                    errores.Add("Falta el campo 'payments' en 'purchase_units'.");

                else if (!payments.TryGetProperty("captures", out var captures) || captures.GetArrayLength() == 0)
                    errores.Add("Falta 'captures' o está vacío dentro de 'payments'.");
                else
                {
                    var capture = captures[0];

                    if (!capture.TryGetProperty("id", out _))
                        errores.Add("Falta el campo 'id' del capture.");

                    if (!capture.TryGetProperty("amount", out var amount))
                        errores.Add("Falta el campo 'amount'.");
                    else
                    {
                        if (!amount.TryGetProperty("value", out _))
                            errores.Add("Falta el campo 'value' dentro de 'amount'.");

                        if (!amount.TryGetProperty("currency_code", out _))
                            errores.Add("Falta el campo 'currency_code' dentro de 'amount'.");
                    }
                }
            }

            if (errores.Count > 0)
                throw new Exception("Errores en el JSON de PayPal:\n" + string.Join("\n", errores));
        }

    }

}

