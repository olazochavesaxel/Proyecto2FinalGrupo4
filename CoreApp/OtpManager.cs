using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using CoreApp;
using DTO;

namespace FinancialWebApp.Utils
{
    public static class OtpManager
    {
        private static Dictionary<string, (string Otp, DateTime ExpiraEn)> otpStorage = new();

        public static string GenerarOtp()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4];
                rng.GetBytes(data);
                int otp = (int)(BitConverter.ToUInt32(data, 0) % 1000000);
                return otp.ToString("D6");
            }
        }

        public static void EnviarOTPcorreo(string email)
        {
            string otp = GenerarOtp();
            otpStorage[email] = (otp, DateTime.UtcNow.AddMinutes(1));

            var smtpClient = new SmtpClient("smtp.tudominio.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tuemail@dominio.com", "tucontraseña"),
                EnableSsl = true,
            };

            var message = new MailMessage
            {
                From = new MailAddress("tuemail@dominio.com"),
                Subject = "Código de recuperación",
                Body = $"Tu código OTP es: {otp}",
                IsBodyHtml = false,
            };

            message.To.Add(email);
            smtpClient.Send(message);
        }

        public static bool ValidarOtp(string email, string otpIngresado)
        {
            if (!otpStorage.ContainsKey(email)) return false;

            var (otpGuardado, expiracion) = otpStorage[email];

            if (DateTime.UtcNow > expiracion)
            {
                otpStorage.Remove(email);
                return false;
            }

            return otpGuardado == otpIngresado;
        }

        public static void ActualizarContrasenna(string email, string nuevaContraseña)
        {
            if (!otpStorage.ContainsKey(email))
                throw new Exception("No hay OTP registrado para este correo.");

            // Aquí deberías actualizar la contraseña en tu base de datos real
            Console.WriteLine($"Contraseña de {email} actualizada a: {nuevaContraseña}");

            otpStorage.Remove(email);
        }

        public static void RemoverOtp(string email)
        {
            if (otpStorage.ContainsKey(email))
                otpStorage.Remove(email);
        }

        public static void EnviarOtpSiUsuarioExiste(string correo)
        {
            var asesorManager = new AsesorManager();
            var clienteManager = new ClienteManager();
            var adminManager = new AdminManager();

            // Verificar si el usuario es un Administrador
            var admin = adminManager.RetrieveByCorreo(correo);
            if (admin != null && admin.Rol == "Admin")
            {
                EnviarOTPcorreo(admin.Correo);
                return;
            }

            // Verificar si el usuario es un Cliente
            var cliente = clienteManager.RetrieveByCorreo(correo);
            if (cliente != null && cliente.Rol == "Cliente")
            {
                EnviarOTPcorreo(cliente.Correo);
                return;
            }

            // Verificar si el usuario es un Asesor
            var asesor = asesorManager.RetrieveByCorreo(correo);
            if (asesor != null && asesor.Rol == "Asesor")
            {
                EnviarOTPcorreo(asesor.Correo);
                return;
            }

            // Si no se encuentra ningún usuario válido
            throw new Exception("No existe un usuario registrado con ese correo.");
        }

    }


}