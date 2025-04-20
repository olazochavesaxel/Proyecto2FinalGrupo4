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
                if (!IngresoComisionesOver0(user.IngresoComisiones))
                    throw new ArgumentException("El ingreso de las comisiones no puede ser menor a 0.");

                Asesor usuarioExistente = RetrieveByCorreo(user.Correo);
                if (Validaciones.UsuarioRegistrado(usuarioExistente, user.Rol))
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");

                user.Contrasenna = Validaciones.HashPassword(user.Contrasenna);
                if (user.FechaExpiracionOTP < new DateTime(1753, 1, 1))
                {
                    user.FechaExpiracionOTP = DateTime.Now.AddMinutes(5);
                }

                usuarioCrud.Create(user);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public void Update(Asesor usuario, bool contrasenaYaHasheada)
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

                if (!IngresoComisionesOver0(usuario.IngresoComisiones))
                    throw new ArgumentException("El ingreso de las comisiones no puede ser menor a 0.");

                if (usuario.FechaExpiracionOTP < new DateTime(1753, 1, 1))
                {
                    usuario.FechaExpiracionOTP = DateTime.Now.AddMinutes(5);
                }

                Asesor usuarioExistente = RetrieveByCorreo(usuario.Correo);
                if (usuarioExistente != null && usuarioExistente.Id != usuario.Id && Validaciones.UsuarioRegistrado(usuarioExistente, usuario.Rol))
                {
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");
                }

                usuarioCrud.Update(usuario);
            }
            catch (Exception ex)
            {
                ManageException(ex);
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

        private bool IngresoComisionesOver0(double IngresoComisiones)
        {
            if (IngresoComisiones < 0)
            {
                return false;
            }
            return true;
        }
    }
}
