using System;
using CoreApp;
using DataAccess.CRUDs;
using DTO;
using Newtonsoft.Json;

class Program
{
    public static void Main(string[] args)
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

        Usuario nuevoUsuario = new Usuario
        {
            Cedula = cedula,
            Nombre = nombre,
            PrimerApellido = primerApellido,
            SegundoApellido = segundoApellido,
            Direccion = direccion,
            Telefono = telefono,
            Correo = correo,
            Contrasenna = contrasenna,
            Estado = estado,
            Rol = rol,
            FechaNacimiento = fechaNacimiento,
            FechaExpiracionOTP = fechaExpiracionOTP
        };

        UsuarioCrudFactory usuarioCrud = new UsuarioCrudFactory();
        usuarioCrud.Create(nuevoUsuario);

        Console.WriteLine("Usuario creado exitosamente.");
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
