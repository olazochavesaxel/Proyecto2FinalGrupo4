using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;
using FinancialWebApp.Utils;

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
            try
            {
                if (user == null) throw new ArgumentException("El usuario no puede ser nulo.");
                if (string.IsNullOrEmpty(user.Nombre)) throw new ArgumentException("El nombre no puede estar vacío.");
                if (string.IsNullOrEmpty(user.Correo)) throw new ArgumentException("El correo no puede estar vacío.");
                if (string.IsNullOrEmpty(user.Contrasenna)) throw new ArgumentException("La contraseña no puede estar vacía.");
                if (string.IsNullOrEmpty(user.Telefono)) throw new ArgumentException("El teléfono no puede estar vacío.");
                if (string.IsNullOrEmpty(user.Cedula)) throw new ArgumentException("La cédula no puede estar vacía.");

                if (!Validaciones.ValidarCorreo(user.Correo))
                    throw new ArgumentException("El formato del correo es inválido.");
                if (!Validaciones.ValidarContrasenna(user.Contrasenna))
                    throw new ArgumentException("La contraseña no cumple con los requisitos mínimos de seguridad.");
                if (!Validaciones.ValidarTelefono(user.Telefono))
                    throw new ArgumentException("El teléfono ingresado no es válido.");
                if (!Validaciones.ValidarCedula(user.Cedula))
                    throw new ArgumentException("La cédula ingresada no es válida.");

                Admin usuarioExistente = RetrieveByCorreo(user.Correo);
                if (Validaciones.UsuarioRegistrado(usuarioExistente, user.Rol))
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");

                user.Contrasenna = Validaciones.HashPassword(user.Contrasenna);
                usuarioCrud.Create(user);
                //OtpManager.EnviarOTPcorreo(user.Correo);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }


        public void Update(Admin usuario, bool contrasenaYaHasheada = false)

        {
            try
            {
                if (usuario == null) throw new ArgumentException("El usuario no puede ser nulo.");
                if (string.IsNullOrEmpty(usuario.Nombre)) throw new ArgumentException("El nombre no puede estar vacío.");
                if (string.IsNullOrEmpty(usuario.Correo)) throw new ArgumentException("El correo no puede estar vacío.");
                if (string.IsNullOrEmpty(usuario.Telefono)) throw new ArgumentException("El teléfono no puede estar vacío.");
                if (string.IsNullOrEmpty(usuario.Cedula)) throw new ArgumentException("La cédula no puede estar vacía.");

                if (!Validaciones.ValidarCorreo(usuario.Correo))
                    throw new ArgumentException("El formato del correo es inválido.");
                if (!Validaciones.ValidarTelefono(usuario.Telefono))
                    throw new ArgumentException("El teléfono ingresado no es válido.");
                if (!Validaciones.ValidarCedula(usuario.Cedula))
                    throw new ArgumentException("La cédula ingresada no es válida.");

                if (!string.IsNullOrEmpty(usuario.Contrasenna))
                {
                    if (!contrasenaYaHasheada && !Validaciones.ValidarContrasenna(usuario.Contrasenna))
                        throw new ArgumentException("La contraseña no cumple con los requisitos mínimos de seguridad.");

                    if (!contrasenaYaHasheada)
                        usuario.Contrasenna = Validaciones.HashPassword(usuario.Contrasenna);
                }


                Admin usuarioExistente = RetrieveByCorreo(usuario.Correo);
                if (usuarioExistente != null && usuarioExistente.Id != usuario.Id && Validaciones.UsuarioRegistrado(usuarioExistente, usuario.Rol))
                {
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");
                }

                usuarioCrud.Update(usuario);
                //OtpManager.EnviarOTPcorreo(usuario.Correo);
            }
            catch (Exception ex)
            {
                ManageException(ex);
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
            return usuarioCrud.RetrieveByCedula<Admin>(cedula);
        }

        public Admin RetrieveByCorreo(string correo)
        {
            return usuarioCrud.RetrieveByCorreo<Admin>(correo);
        }

        // Validaciones
        protected bool IsOver18(Admin usuario)
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

