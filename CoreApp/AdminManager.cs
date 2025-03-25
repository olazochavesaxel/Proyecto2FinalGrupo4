using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class AdminManager : BaseManager
    {
        private readonly AdminCrudFactory usuarioCrud;

        public AdminManager()
        {
            usuarioCrud = new AdminCrudFactory();  // Initialize UserCrudFactory
        }

        public void Create(Admin user)
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

        public void Update(Admin usuario)
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

        public void Delete(Admin usuario)
        {
            usuarioCrud.Delete(usuario);
        }

        public List<Admin> RetrieveAll()
        {
            return usuarioCrud.RetrieveAll<Admin>();
        }

        public Admin RetrieveById(int id)
        {
            return usuarioCrud.RetrieveById<Admin>(id);
        }

        public Admin RetrieveByCedula(string cedula)
        {
            return usuarioCrud.RetrieveByCedula<Admin   >(cedula);
        }

        public Admin RetrieveByCorreo(string correo)
        {
            return usuarioCrud.RetrieveByCorreo<Admin>(correo);
        }

        // Validaciones
        protected bool IsOver18(Admin  usuario)
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
