using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class ClienteManager : BaseManager
    {
        private readonly ClienteCrudFactory usuarioCrud;

        public ClienteManager()
        {
            usuarioCrud = new ClienteCrudFactory();
        }

        public void Create(Cliente user)
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
                if (!balanceOver0(user.BalanceFinanciero))
                    throw new ArgumentException("El balance no puede ser menor a 0.");

                Cliente usuarioExistente = RetrieveByCorreo(user.Correo);
                if (Validaciones.UsuarioRegistrado(usuarioExistente, user.Rol))
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");

                user.Contrasenna = Validaciones.HashPassword(user.Contrasenna);

                if (user.FechaExpiracionOTP < new DateTime(1753, 1, 1))
                {
                    user.FechaExpiracionOTP = DateTime.Now.AddMinutes(5);
                }

                usuarioCrud.Create(user);

                // Obtener ID del nuevo cliente
                int idClienteCreado = RetrieveByCorreo(user.Correo).Id;

                // Obtener lista de asesores y asignar uno al azar
                var asesorCrud = new AsesorCrudFactory();
                var asesores = asesorCrud.RetrieveAll<Asesor>();

                if (asesores != null && asesores.Count > 0)
                {
                    var random = new Random();
                    int index = random.Next(asesores.Count);
                    int idAsesor = asesores[index].Id;

                    usuarioCrud.AsignarAsesor(idClienteCreado, idAsesor);
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public void Update(Cliente usuario)
        {
            Update(usuario, false);
        }

        public void Update(Cliente usuario, bool contrasenaYaHasheada)
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

                if (!balanceOver0(usuario.BalanceFinanciero))
                    throw new ArgumentException("El balance no puede ser menor a 0.");

                if (usuario.FechaExpiracionOTP < new DateTime(1753, 1, 1))
                {
                    usuario.FechaExpiracionOTP = DateTime.Now.AddMinutes(5);
                }

                Cliente usuarioExistente = RetrieveById(usuario.Id);
                if (usuarioExistente != null && usuarioExistente.Id != usuario.Id && Validaciones.UsuarioRegistrado(usuarioExistente, usuario.Rol))
                {
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");
                }

                // Proteger el rol
                usuario.Rol = usuarioExistente?.Rol ?? "Cliente";

                usuarioCrud.Update(usuario);
            }
            catch (Exception ex)
            {
                ManageException(ex);
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

        public List<Cliente> RetrieveClientesAsignados(int idAsesor)
        {
            return usuarioCrud.RetrieveClientesAsignados<Cliente>(idAsesor);
        }

        public void AsignarAsesor(int idCliente, int idAsesor)
        {
            try
            {
                usuarioCrud.AsignarAsesor(idCliente, idAsesor);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private bool balanceOver0(double balance)
        {
            return balance >= 0;
        }

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
