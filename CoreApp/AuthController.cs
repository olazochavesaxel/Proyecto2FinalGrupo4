using System;
using DTO;

namespace CoreApp
{
    public class AuthManager
    {
        public static Usuario ValidarUsuario(string correo, string contrasena)
        {
            Console.WriteLine(Validaciones.HashPassword("TuContrasena123!"));
            var asesorManager = new AsesorManager();
            var clienteManager = new ClienteManager();
            var adminManager = new AdminManager();

            // Verificar si el usuario es un Administrador
            var admin = adminManager.RetrieveByCorreo(correo);
            if (Validaciones.UsuarioRegistrado(admin, "Admin") && Validaciones.ContrasennaCorrecta(admin.Contrasenna, contrasena))
            {
                //NotificationManager.EnviarNotificacion(admin, "Acceso exitoso", "Bienvenido, Administrador.");
                return admin;
            }

            // Verificar si el usuario es un Cliente
            var cliente = clienteManager.RetrieveByCorreo(correo);
            if (Validaciones.UsuarioRegistrado(cliente, "Cliente") && Validaciones.ContrasennaCorrecta(cliente.Contrasenna, contrasena))
            {
                //NotificationManager.EnviarNotificacion(cliente, "Acceso exitoso", "Bienvenido, Cliente.");
                return cliente;
            }

            // Verificar si el usuario es un Asesor
            var asesor = asesorManager.RetrieveByCorreo(correo);
            if (Validaciones.UsuarioRegistrado(asesor, "Asesor") && Validaciones.ContrasennaCorrecta(asesor.Contrasenna, contrasena))
            {
                //NotificationManager.EnviarNotificacion(asesor, "Acceso exitoso", "Bienvenido, Asesor.");
                return asesor;
            }

            return null; // Usuario no encontrado o contraseña incorrecta
        }
    }
}