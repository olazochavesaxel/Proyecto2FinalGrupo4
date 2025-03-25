using System;
using DTO;

namespace CoreApp
{
    public class AuthManager
    {
        public static Usuario ValidarUsuario(string correo, string contrasena)
        {
            var asesorManager = new AsesorManager();
            var clienteManager = new ClienteManager();
            var adminManager = new AdminManager();

            // Verificar si el usuario es un Administrador
            var admin = adminManager.RetrieveByCorreo(correo);
            if (admin != null && admin.Contrasenna == contrasena && admin.Rol == "Admin")
            {
                // Llamar a NotificationManager para enviar la notificación
                NotificationManager.EnviarNotificacion(admin, "Acceso exitoso", "Bienvenido, Administrador.");
                return admin;
            }

            // Verificar si el usuario es un Cliente
            var cliente = clienteManager.RetrieveByCorreo(correo);
            if (cliente != null && cliente.Contrasenna == contrasena && cliente.Rol == "Cliente")
            {
                // Llamar a NotificationManager para enviar la notificación
                NotificationManager.EnviarNotificacion(cliente, "Acceso exitoso", "Bienvenido, Cliente.");
                return cliente;
            }

            // Verificar si el usuario es un Asesor
            var asesor = asesorManager.RetrieveByCorreo(correo);
            if (asesor != null && asesor.Contrasenna == contrasena && asesor.Rol == "Asesor")
            {
                // Llamar a NotificationManager para enviar la notificación
                NotificationManager.EnviarNotificacion(asesor, "Acceso exitoso", "Bienvenido, Asesor.");
                return asesor;
            }

            return null; // Usuario no encontrado o contraseña incorrecta
        }
    }
}