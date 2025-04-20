using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PasswordRecoveryManager
    {
        public static void ActualizarContrasena(string correo, string nuevaContrasena)
        {
            if (string.IsNullOrEmpty(correo))
                throw new ArgumentException("El correo no puede estar vacío.");
            if (string.IsNullOrEmpty(nuevaContrasena))
                throw new ArgumentException("La nueva contraseña no puede estar vacía.");
            if (!Validaciones.ValidarContrasenna(nuevaContrasena))
                throw new ArgumentException("La contraseña no cumple con los requisitos mínimos de seguridad.");

            var hashedPassword = Validaciones.HashPassword(nuevaContrasena);

            var adminManager = new AdminManager();
            var admin = adminManager.RetrieveByCorreo(correo);
            if (admin != null)
            {
                admin.Contrasenna = hashedPassword;
                adminManager.Update(admin, true); // true = ya viene hasheada
                return;
            }

            var clienteManager = new ClienteManager();
            var cliente = clienteManager.RetrieveByCorreo(correo);
            if (cliente != null)
            {
                cliente.Contrasenna = hashedPassword;
                clienteManager.Update(cliente, true);
                return;
            }

            var asesorManager = new AsesorManager();
            var asesor = asesorManager.RetrieveByCorreo(correo);
            if (asesor != null)
            {
                asesor.Contrasenna = hashedPassword;
                asesorManager.Update(asesor, true);
                return;
            }

            throw new ArgumentException("No se encontró un usuario con ese correo.");
        }

    }
}
