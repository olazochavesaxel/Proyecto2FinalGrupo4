using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DTO;

namespace CoreApp
{
    public static class Validaciones
    {
        private static Dictionary<string, (string Otp, DateTime ExpiraEn)> otpStorage = new();

        //**************************Validaciones de datos
        public static bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo)) return false;
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }

        public static bool ValidarContrasenna(string contrasenna)
        {
            if (string.IsNullOrEmpty(contrasenna) || contrasenna.Length <= 8) return false;
            string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(contrasenna, patron);
        }

        public static bool ValidarTelefono(string telefono)
        {
            if (string.IsNullOrEmpty(telefono) || telefono.Length < 8 || telefono.Length > 11) return false;
            string patron = @"^\d{8,11}$";
            return Regex.IsMatch(telefono, patron);
        }

        public static bool ValidarCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula) || cedula.Length < 5 || cedula.Length > 9) return false;
            string patron = @"^\d{5,9}$";
            return Regex.IsMatch(cedula, patron);
        }

        //**************************Validaciones de Autentificacion
        

        public static bool ContrasennaCorrecta(string contrasennaGuardada, string contrasennaIngresada)
        {
            return contrasennaGuardada != null && contrasennaGuardada == HashPassword(contrasennaIngresada);
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool UsuarioRegistrado(Usuario usuarioExistente, string rol)
        {
            return usuarioExistente != null && usuarioExistente.Rol == rol;
        }

        public static bool IniciarSesion(string correo, string contrasenna, UsuarioManager usuarioManager)
        {
            Usuario usuario = usuarioManager.RetrieveByCorreo(correo);
            if (usuario == null) return false;
            return ContrasennaCorrecta(usuario.Contrasenna, contrasenna);
        }


        //**************************Validaciones OTP
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
            if (!otpStorage.ContainsKey(email) || otpStorage[email].ExpiraEn < DateTime.UtcNow)
                return false;
            return otpStorage[email].Otp == otpIngresado;
        }

        public static void ActualizarContrasenna(string email, string nuevaContraseña)
        {
            if (!otpStorage.ContainsKey(email))
                throw new Exception("No hay OTP registrado para este correo.");

            Console.WriteLine($"Contraseña de {email} actualizada a: {nuevaContraseña}");
            otpStorage.Remove(email);
        }
    }
}
