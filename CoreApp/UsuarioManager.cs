using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

                Usuario usuarioExistente = RetrieveByCorreo(user.Correo);
                if (Validaciones.UsuarioRegistrado(usuarioExistente, user.Rol))
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");

                user.Contrasenna = Validaciones.HashPassword(user.Contrasenna);
                usuarioCrud.Create(user);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }



        public void Update(Usuario usuario)
        {
            try
            {
                // Validar datos obligatorios
                if (usuario == null) throw new ArgumentException("El usuario no puede ser nulo.");
                if (string.IsNullOrEmpty(usuario.Nombre)) throw new ArgumentException("El nombre no puede estar vacío.");
                if (string.IsNullOrEmpty(usuario.Correo)) throw new ArgumentException("El correo no puede estar vacío.");
                if (string.IsNullOrEmpty(usuario.Telefono)) throw new ArgumentException("El teléfono no puede estar vacío.");
                if (string.IsNullOrEmpty(usuario.Cedula)) throw new ArgumentException("La cédula no puede estar vacía.");

                // Validar formato de correo
                if (!Validaciones.ValidarCorreo(usuario.Correo))
                    throw new ArgumentException("El formato del correo es inválido.");

                // Validar teléfono
                if (!Validaciones.ValidarTelefono(usuario.Telefono))
                    throw new ArgumentException("El teléfono ingresado no es válido.");

                // Validar cédula
                if (!Validaciones.ValidarCedula(usuario.Cedula))
                    throw new ArgumentException("La cédula ingresada no es válida.");

                // Si la contraseña ha sido cambiada, validar la contraseña
                if (!string.IsNullOrEmpty(usuario.Contrasenna) && !Validaciones.ValidarContrasenna(usuario.Contrasenna))
                    throw new ArgumentException("La contraseña no cumple con los requisitos mínimos de seguridad.");

                // Si la contraseña es válida, se hashea antes de actualizar
                if (!string.IsNullOrEmpty(usuario.Contrasenna))
                {
                    usuario.Contrasenna = Validaciones.HashPassword(usuario.Contrasenna);
                }

                // Si el usuario está tratando de cambiar el correo o el rol, asegurarse que no exista otro usuario con ese correo y rol
                Usuario usuarioExistente = RetrieveByCorreo(usuario.Correo);
                if (usuarioExistente != null && usuarioExistente.Id != usuario.Id && Validaciones.UsuarioRegistrado(usuarioExistente, usuario.Rol))
                {
                    throw new ArgumentException("El usuario ya está registrado con este correo y rol.");
                }

                // Actualizar el usuario en la base de datos
                usuarioCrud.Update(usuario);
            }
            catch (Exception ex)
            {
                ManageException(ex);
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

        public Usuario RetrieveByCedula(int cedula)
        {
            return usuarioCrud.RetrieveByCedula<Usuario>(cedula);
        }

        //Devulve un usuario por medio del correo
        public Usuario RetrieveByCorreo(string correo)
        {
            return usuarioCrud.RetrieveByCorreo<Usuario>(correo);
        }

        public bool IniciarSesion(string correo, string contrasenna)
        {
            return Validaciones.IniciarSesion(correo, contrasenna, this);
        }

        //Nueva linea//////////////////////////////////////////////////
        public void ActualizarContrasenaPorCorreo(string correo, string nuevaContrasena)
        {
            try
            {
                if (string.IsNullOrEmpty(correo))
                    throw new ArgumentException("El correo no puede estar vacío.");
                if (string.IsNullOrEmpty(nuevaContrasena))
                    throw new ArgumentException("La nueva contraseña no puede estar vacía.");
                if (!Validaciones.ValidarContrasenna(nuevaContrasena))
                    throw new ArgumentException("La nueva contraseña no cumple con los requisitos mínimos de seguridad.");

                var usuario = RetrieveByCorreo(correo);
                if (usuario == null)
                    throw new ArgumentException("No se encontró un usuario con ese correo.");

                usuario.Contrasenna = Validaciones.HashPassword(nuevaContrasena);
                usuarioCrud.Update(usuario);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

    }
}