using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class AsesorManager : BaseManager
    {
        private readonly AsesorCrudFactory usuarioCrud;

        public AsesorManager()
        {
            usuarioCrud = new AsesorCrudFactory();  // Initialize UserCrudFactory
        }

        public void Create(Asesor user)
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

        public void Update(Asesor usuario)
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

        public void Delete(Asesor usuario)
        {
            usuarioCrud.Delete(usuario);
        }

        public List<Asesor> RetrieveAll()
        {
            return usuarioCrud.RetrieveAll<Asesor>();
        }

        public Asesor RetrieveById(int id)
        {
            return usuarioCrud.RetrieveById<Asesor>(id);
        }

        public Asesor RetrieveByCedula(string cedula)
        {
            return usuarioCrud.RetrieveByCedula<Asesor>(cedula);
        }

        public Asesor RetrieveByCorreo(string correo)
        {
            return usuarioCrud.RetrieveByCorreo<Asesor>(correo);
        }

        // Validaciones
        private bool IsOver18(Asesor usuario)
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
