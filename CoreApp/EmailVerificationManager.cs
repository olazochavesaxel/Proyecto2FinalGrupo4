using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DTO;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CoreApp
{
    public class EmailVerificationManager
    {
        private static string apiKey = "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"; // Reemplaza con tu clave real o mejor, usa una variable de entorno


        /////////////////////////////Linea nueva diccionario para guardar codigos otp//////////////////////////////////////
        private static Dictionary<string, string> otpsPorCorreo = new Dictionary<string, string>();// <- <- <- <- <- <-

        public static async Task<Usuario> ValidarCorreoYEnviarOtp(string correo)
        {
            var asesorManager = new AsesorManager();
            var clienteManager = new ClienteManager();
            var adminManager = new AdminManager();

            // Verificar si el correo existe en la base de datos para cada tipo de usuario
            var admin = adminManager.RetrieveByCorreo(correo);
            if (admin != null)
            {
                var otpCode = GenerarOtp();
                otpsPorCorreo[correo] = otpCode;//Linea nueva almacen del otp // <- <- <- <- <- <-
                await EnviarOtpCorreo(admin.Correo, otpCode);
                return admin;
            }

            var cliente = clienteManager.RetrieveByCorreo(correo);
            if (cliente != null)
            {
                var otpCode = GenerarOtp();
                otpsPorCorreo[correo] = otpCode;//Linea nueva almacen del otp // <- <- <- <- <- <-

                await EnviarOtpCorreo(cliente.Correo, otpCode);
                return cliente;
            }

            var asesor = asesorManager.RetrieveByCorreo(correo);
            if (asesor != null)
            {
                var otpCode = GenerarOtp();
                otpsPorCorreo[correo] = otpCode;//Linea nueva almacen del otp // <- <- <- <- <- <-

                await EnviarOtpCorreo(asesor.Correo, otpCode);
                return asesor;
            }

            return null; // Usuario no encontrado
        }

        private static string GenerarOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private static async Task EnviarOtpCorreo(string correoDestinatario, string otpCode)
        {
            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("msotoba@ucenfotec.ac.cr", "TuNombre o Empresa"); // Usa un correo verificado de SendGrid
                var to = new EmailAddress(correoDestinatario);
                var subject = "Tu código OTP";
                var plainTextContent = $"Tu código OTP es: {otpCode}";
                var htmlContent = $"<strong>Tu código OTP es: {otpCode}</strong>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("OTP enviado correctamente.");
                }
                else
                {
                    Console.WriteLine($"Error al enviar OTP. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al enviar el OTP: {ex.Message}");
            }
        }


        /////////////////////////////Linea nueva Validar codigos otp//////////////////////////////////////
        public static bool VerificarOtp(string correo, string otpIngresado)
        {
            if (otpsPorCorreo.TryGetValue(correo, out string otpGuardado))
            {
                if (otpGuardado == otpIngresado)
                {
                    otpsPorCorreo.Remove(correo); // ❌ Elimina el OTP si es válido
                    return true;
                }
            }

            return false;
        }



    }
}
