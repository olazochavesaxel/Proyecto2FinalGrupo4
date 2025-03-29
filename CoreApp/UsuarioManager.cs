using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    // Capa lógica y manejo de excepciones para DTO User
    public class UsuarioManager : BaseManager
    {
        private readonly UsuarioCrudFactory usuarioCrud;

        public UsuarioManager()
        {
            usuarioCrud = new UsuarioCrudFactory();  // Initialize UserCrudFactory
        }

        public void Create(Usuario user)
        {
            try
            {
                // Validar datos obligatorios
                if (user == null)
                    throw new ArgumentException("El usuario no puede ser nulo.");
                if (string.IsNullOrEmpty(user.Nombre))
                    throw new ArgumentException("El nombre no puede estar vacío.");
                if (string.IsNullOrEmpty(user.Correo))
                    throw new ArgumentException("El correo no puede estar vacío.");
                if (string.IsNullOrEmpty(user.Contrasenna))
                    throw new ArgumentException("La contraseña no puede estar vacía.");
                if (string.IsNullOrEmpty(user.Telefono))
                    throw new ArgumentException("El teléfono no puede estar vacío.");
                if (string.IsNullOrEmpty(user.Cedula))
                    throw new ArgumentException("La cédula no puede estar vacía.");


                // Validar formato de correo
                if (!ValidarCorreo(user.Correo))
                    throw new ArgumentException("El formato del correo es inválido.");

                // Validar contraseña segura
                if (!ValidarContrasenna(user.Contrasenna))
                    throw new ArgumentException("La contraseña no cumple con los requisitos mínimos de seguridad.");

                // Validar teléfono
                if (!ValidarTelefono(user.Telefono))
                    throw new ArgumentException("El teléfono ingresado no es válido.");

                // Validar cédula
                if (!ValidarCedula(user.Cedula))
                    throw new ArgumentException("La cédula ingresada no es válida.");

                // Verificar si el usuario ya está registrado
                if (UsuarioRegistrado(user.Correo, user.Rol))
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");

                // Hashear la contraseña antes de guardarla
                user.Contrasenna = HashPassword(user.Contrasenna);

                // Crear el usuario en la base de datos
                usuarioCrud.Create(user);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }


        public void Update(Usuario usuario)
        {


            usuarioCrud.Update(usuario);

        }

        public void Delete(Usuario usuario)
        {
            usuarioCrud.Delete(usuario);
        }

        public List<Usuario> RetrieveAll()
        {
            return usuarioCrud.RetrieveAll<Usuario>();
        }

        public Usuario RetrieveById(int id)
        {
            return usuarioCrud.RetrieveById<Usuario>(id);
        }

        public Usuario RetrieveByCedula(int cedula)
        {
            return usuarioCrud.RetrieveByCedula<Usuario>(cedula);
        }

        //Devulve un usuario por medio del correo
        public Usuario RetrieveByCorreo(string correo)
        {
            return usuarioCrud.RetrieveByCorreo<Usuario>(correo);
        }

        // Validaciones


        //************Validaciones MELI****************
        //Genera el hash de la contrasenna
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);  // 🔹 Devuelve una cadena de 64 caracteres
            }
        }
        //Verifica que la contrasenna ingresada por el usaurio sea la misma que tiene en BD
        public bool ContrasennaCorrecta(string correo, string contrasennaIngresada)
        {
            string contrasennaGuardada = ObtenerContrasennaBD(correo);
            string contrasennaIngresadaHash = HashPassword(contrasennaIngresada); // 🔹 Encriptar la ingresada
            return contrasennaGuardada != null && contrasennaGuardada == contrasennaIngresadaHash;
        }

        //Obtiene la Contrasenna de BD
        private string ObtenerContrasennaBD(string correo)
        {
            Usuario usuario = RetrieveByCorreo(correo);
            return usuario?.Contrasenna;
        }

        //Verfica si el usuario ya esta registrado en la base de datos
        public bool UsuarioRegistrado(string correo, string rol)
        {
            Usuario usuarioExistente = RetrieveByCorreo(correo);

            // Si no hay usuario con ese correo, el usuario no está registrado
            if (usuarioExistente == null)
            {
                return false;
            }

            // Si el usuario ya tiene el mismo rol, devolvemos true (ya registrado)
            return usuarioExistente.Rol == rol;
        }

        public bool IniciarSesion(string correo, string contrasenna)
        {
            // Verificar si el usuario está registrado
            Usuario usuario = RetrieveByCorreo(correo);
            if (usuario == null)
            {
                Console.WriteLine("El usuario no está registrado.");
                return false;
            }

            // Verificar si la contraseña es correcta
            if (!ContrasennaCorrecta(correo, contrasenna))
            {
                Console.WriteLine("Contraseña incorrecta.");
                return false;
            }

            Console.WriteLine("Inicio de sesión exitoso.");
            return true;
        }


        //Verifica que la contrasenna cumpla con los requerimientos minimos de seguridad
        public bool ValidarContrasenna(string contrasenna)
        {
            if (string.IsNullOrEmpty(contrasenna) || contrasenna.Length <= 8)
            {
                return false;
            }

            // Expresión regular para validar los requisitos
            string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(contrasenna, patron);
        }

        //Verifica que solo se ingresen numeros y que no se ingresen menos de 8 o mas de 11
        public bool ValidarTelefono(string telefono)
        {
            if (string.IsNullOrEmpty(telefono) || telefono.Length < 8 || telefono.Length > 11)
            {
                return false;
            }

            // Expresión regular para validar los requisitos
            string patron = @"^\d{8,11}$";
            return Regex.IsMatch(telefono, patron);
        }

        //Verifica que solo se ingresen numeros y que no se ingresen menos de 5 o mas de 9

        public bool ValidarCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula) || cedula.Length < 5 || cedula.Length > 9)
            {
                return false;
            }

            // Expresión regular para validar los requisitos
            string patron = @"^\d{5,9}$";
            return Regex.IsMatch(cedula, patron);
        }

        public bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                return false;
            }

            // Expresión regular para validar los requisitos
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }


        /////////Validaciones OTP y contraseña////////////
        //////////////////////////////////////////////////

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
            otpStorage[email] = (otp, DateTime.UtcNow.AddMinutes(1)); // OTP válido por 5 minutos

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
                return false; // OTP inválido o expirado

            return otpStorage[email].Otp == otpIngresado;
        }

        public static void ActualizarContrasenna(string email, string nuevaContraseña)
        {
            if (!otpStorage.ContainsKey(email))
                throw new Exception("No hay OTP registrado para este correo.");

            // Aquí iría la lógica para actualizar la contraseña en la base de datos.
            Console.WriteLine($"Contraseña de {email} actualizada a: {nuevaContraseña}");

            // Eliminar el OTP después de usarlo
            otpStorage.Remove(email);
        }




    }
}