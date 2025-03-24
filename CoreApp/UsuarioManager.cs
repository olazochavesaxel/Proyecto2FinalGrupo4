using System;
using System.Collections.Generic;
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

        public void Update(Usuario usuario)
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

        public Usuario RetrieveByCedula(string cedula)
        {
            return usuarioCrud.RetrieveByCedula<Usuario>(cedula);
        }

        // Validaciones
        private bool IsOver18(Usuario usuario)
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
