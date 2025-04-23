using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;
using DTO;
using DTOs;

namespace CoreApp
{
    using DTOs;

    public class TransaccionManager
    {
        private TransaccionCrudFactory crud;
        private UsuarioManager usuarioManager;

        public TransaccionManager()
        {
            crud = new TransaccionCrudFactory();
            usuarioManager = new UsuarioManager();
        }

        public void Create(Transaccion trans)
        {
            // Recuperar el usuario con su rol
            var usuario = usuarioManager.RetrieveById(trans.UserId);

            // Verificar rol y llamar al método correspondiente
            switch (usuario.Rol.ToLower())
            {
                case "cliente":
                    crud.Create_TR_PAYPAL_CLIENTE(trans);
                    break;
                case "asesor":
                    crud.Create_TR_PAYPAL_ASESOR(trans);
                    break;
                case "admin":
                    // No se realiza operación
                    Console.WriteLine("Los administradores no pueden registrar transacciones PayPal.");
                    break;
                default:
                    throw new Exception("Rol de usuario no reconocido.");
            }
        }
    }
}    
