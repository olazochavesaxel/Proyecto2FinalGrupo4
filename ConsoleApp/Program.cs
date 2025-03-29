using System;
using CoreApp;
using DataAccess.CRUDs;
using DTO;
using Newtonsoft.Json;


public class Program
{
    private static UsuarioManager usuarioManager = new UsuarioManager();

    public static void Main(string[] args)
    {
        int opcionMenu = 0;

        while (opcionMenu != 9)
        {
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Registrar usuario");
            Console.WriteLine("2. Iniciar sesión");
            Console.WriteLine("3. Actualizar usuario");
            Console.WriteLine("4. Eliminar usuario");
            Console.WriteLine("5. Listar usuarios");
            Console.WriteLine("6. Buscar usuario por correo");
            Console.WriteLine("7. Buscar usuario por cedula");
            Console.WriteLine("8. Buscar usuario por id");
            Console.WriteLine("9. Salir");

            Console.Write("Opción: ");
            opcionMenu = int.Parse(Console.ReadLine());

            switch (opcionMenu)
            {
                case 1:
                    RegistrarUsuario();
                    break;
                case 2:
                    IniciarSesion();
                    break;
                case 3:
                    ActualizarUsuario();
                    break;
                case 4:
                    EliminarUsuario();
                    break;
                case 5:
                    ListarUsuarios();
                    break;
                case 6:
                    BuscarUsuarioCorreo();
                    break;
                case 7:
                    BuscarUsuarioCedula();
                    break;
                case 8:
                    BuscarUsuarioID();
                    break;
                case 9:
                    Console.WriteLine("¡Gracias por usar el sistema!");
                    break;
                default:
                    Console.WriteLine("Opción no válida, intente de nuevo.");
                    break;
            }
        }
    }

    static void RegistrarUsuario()
    {
        Console.WriteLine("Ingrese la cédula:");
        string cedula = Console.ReadLine();

        Console.WriteLine("Ingrese el nombre:");
        string nombre = Console.ReadLine();

        Console.WriteLine("Ingrese el primer apellido:");
        string primerApellido = Console.ReadLine();

        Console.WriteLine("Ingrese el segundo apellido:");
        string segundoApellido = Console.ReadLine();

        Console.WriteLine("Ingrese la dirección:");
        string direccion = Console.ReadLine();

        Console.WriteLine("Ingrese la foto de perfil:");
        string fotoPerfil = Console.ReadLine();

        Console.WriteLine("Ingrese el teléfono:");
        string telefono = Console.ReadLine();

        Console.WriteLine("Ingrese el correo electrónico:");
        string correo = Console.ReadLine();

        Console.WriteLine("Ingrese la contraseña:");
        string contrasenna = Console.ReadLine();

        Console.WriteLine("Ingrese el estado:");
        string estado = Console.ReadLine();

        Console.WriteLine("Ingrese el rol:");
        string rol = Console.ReadLine();

        Console.WriteLine("Ingrese la fecha de nacimiento (yyyy-MM-dd):");
        DateTime fechaNacimiento;
        while (!DateTime.TryParse(Console.ReadLine(), out fechaNacimiento))
        {
            Console.WriteLine("Formato inválido. Ingrese la fecha de nacimiento en formato yyyy-MM-dd:");
        }

        Console.WriteLine("Ingrese la fecha de expiración OTP (yyyy-MM-dd):");
        DateTime fechaExpiracionOTP;
        while (!DateTime.TryParse(Console.ReadLine(), out fechaExpiracionOTP))
        {
            Console.WriteLine("Formato inválido. Ingrese la fecha de expiración OTP en formato yyyy-MM-dd:");
        }

        var um = new UsuarioManager();
        Usuario nuevoUsuario = new Usuario
        {
            Cedula = cedula,
            Nombre = nombre,
            PrimerApellido = primerApellido,
            SegundoApellido = segundoApellido,
            Direccion = direccion,
            FotoPerfil = fotoPerfil,
            Telefono = telefono,
            Correo = correo,
            Contrasenna = contrasenna,
            Estado = estado,
            Rol = rol,
            FechaNacimiento = fechaNacimiento,
            FechaExpiracionOTP = fechaExpiracionOTP
        };

        um.Create(nuevoUsuario);

        Console.WriteLine("Usuario creado exitosamente.");
    }


    //Iniciar Sesion//////////////////////////////////////////
    static void IniciarSesion()
    {
        Console.Write("Ingrese su correo: ");
        string correo = Console.ReadLine();
        Console.Write("Ingrese su contraseña: ");
        string contrasenna = Console.ReadLine();

        if (usuarioManager.IniciarSesion(correo, contrasenna))
        {
            Console.WriteLine("Inicio de sesión exitoso.");
        }
        else
        {
            Console.WriteLine("Error en el inicio de sesión.");
        }
    }


    // Actualizar Usuario//////////////////////////////////////////
    static void ActualizarUsuario()
    {
        Console.Write("Ingrese el correo del usuario a actualizar: ");
        string correo = Console.ReadLine();
        Usuario usuario = usuarioManager.RetrieveByCorreo(correo);

        if (usuario == null)
        {
            Console.WriteLine("Usuario no encontrado.");
            return;
        }

        Console.WriteLine("Ingrese la cédula:");
        string cedula = Console.ReadLine();

        Console.WriteLine("Ingrese el nombre:");
        string nombre = Console.ReadLine();

        Console.WriteLine("Ingrese el primer apellido:");
        string primerApellido = Console.ReadLine();

        Console.WriteLine("Ingrese el segundo apellido:");
        string segundoApellido = Console.ReadLine();

        Console.WriteLine("Ingrese la dirección:");
        string direccion = Console.ReadLine();

        Console.WriteLine("Ingrese la foto de perfil:");
        string fotoPerfil = Console.ReadLine();

        Console.WriteLine("Ingrese el teléfono:");
        string telefono = Console.ReadLine();

        Console.WriteLine("Ingrese el correo electrónico:");
        string correoActualizado = Console.ReadLine();

        Console.WriteLine("Ingrese la contraseña:");
        string contrasenna = Console.ReadLine();

        Console.WriteLine("Ingrese el estado:");
        string estado = Console.ReadLine();

        Console.WriteLine("Ingrese el rol:");
        string rol = Console.ReadLine();

        Console.WriteLine("Ingrese la fecha de nacimiento (yyyy-MM-dd):");
        DateTime fechaNacimiento;
        while (!DateTime.TryParse(Console.ReadLine(), out fechaNacimiento))
        {
            Console.WriteLine("Formato inválido. Ingrese la fecha de nacimiento en formato yyyy-MM-dd:");
        }

        Console.WriteLine("Ingrese la fecha de expiración OTP (yyyy-MM-dd):");
        DateTime fechaExpiracionOTP;
        while (!DateTime.TryParse(Console.ReadLine(), out fechaExpiracionOTP))
        {
            Console.WriteLine("Formato inválido. Ingrese la fecha de expiración OTP en formato yyyy-MM-dd:");
        }

        var um = new UsuarioManager();
        Usuario nuevoUsuario = new Usuario
        {
            Cedula = cedula,
            Nombre = nombre,
            PrimerApellido = primerApellido,
            SegundoApellido = segundoApellido,
            Direccion = direccion,
            FotoPerfil = fotoPerfil,
            Telefono = telefono,
            Correo = correoActualizado,
            Contrasenna = contrasenna,
            Estado = estado,
            Rol = rol,
            FechaNacimiento = fechaNacimiento,
            FechaExpiracionOTP = fechaExpiracionOTP
        };
        um.Update(nuevoUsuario);

        UsuarioCrudFactory usuarioCrud = new UsuarioCrudFactory();
        usuarioCrud.Create(nuevoUsuario);

        Console.WriteLine("Usuario actualizado exitosamente.");
    }
    ///EliminarUsuario///////////////////////////////
    static void EliminarUsuario()
    {
        Console.Write("Ingrese el id del usuario a eliminar: ");
        int id = int.Parse(Console.ReadLine());
        var um = new UsuarioManager();
        um.Delete(new Usuario { Id = id });

        Console.WriteLine("Usuario eliminado exitosamente.");
    }

    static void ListarUsuarios()
    {
        Console.WriteLine("****** Listado de Usuarios ******");
        var um = new UsuarioManager();
        um.RetrieveAll();
    }

    static void BuscarUsuarioID()
    {
        Console.Write("Ingrese el ID del usuario: ");
        int id = int.Parse(Console.ReadLine());
        var um = new UsuarioManager();
        var usuario = um.RetrieveById(id);
        Console.WriteLine(JsonConvert.SerializeObject(usuario));
    }

    static void BuscarUsuarioCedula()
    {
        Console.Write("Ingrese la cedula del usuario: ");
        int cedula = int.Parse(Console.ReadLine());
        var um = new UsuarioManager();
        var usuario = um.RetrieveByCedula(cedula);
        Console.WriteLine(JsonConvert.SerializeObject(usuario));
    }

    static void BuscarUsuarioCorreo()
    {
        Console.Write("Ingrese el correo del usuario: ");
        string correo = Console.ReadLine();
        var um = new UsuarioManager();
        var usuario = um.RetrieveByCorreo(correo);
        Console.WriteLine(JsonConvert.SerializeObject(usuario));
    }



}




