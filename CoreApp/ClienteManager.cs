using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class ClienteManager: BaseManager
    {
        private readonly ClienteCrudFactory usuarioCrud;

        public ClienteManager()
        {
            usuarioCrud = new ClienteCrudFactory();  // Initialize UserCrudFactory
        }

        public void Create(Cliente user)
        {
            // Aplicar validación de la edad del usuario al registrar (edad >= 18)
            if (IsOver18(user))
            {
                usuarioCrud.Create(user);
            }
            else
            {
                ManageException(new Exception("User is not over 18"));
            }
        }

        public void Update(Cliente usuario)
        {
            if (IsOver18(usuario))
            {
                usuarioCrud.Update(usuario);
            }
            else
            {
                ManageException(new Exception("User is not over 18"));
            }
        }

        public void Delete(Cliente usuario)
        {
            usuarioCrud.Delete(usuario);
        }

        public List<Cliente> RetrieveAll()
        {
            return usuarioCrud.RetrieveAll<Cliente>();
        }

        public Cliente RetrieveById(int id)
        {
            return usuarioCrud.RetrieveById<Cliente>(id);
        }

        public Cliente RetrieveByCedula(string cedula)
        {
            return usuarioCrud.RetrieveByCedula<Cliente>(cedula);
        }

        public Cliente RetrieveByCorreo(string correo)
        {
            return usuarioCrud.RetrieveByCorreo<Cliente>(correo);
        }

        // Validaciones
        private bool IsOver18(Cliente usuario)
        {
            var currentDate = DateTime.Now;
            int age = currentDate.Year - usuario.FechaNacimiento.Year;
            if (usuario.FechaNacimiento.Date > currentDate.AddYears(-age).Date)
            {
                age--;
            }
            return age >= 18;
        }
    }
}
