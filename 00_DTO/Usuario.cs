using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Usuario : BaseDTO
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Direccion { get; set; }
        public string FotoPerfil { get; set; }
        public string Contrasenna { get; set; }
        public string Telefono { get; set; }  
        public string Estado { get; set; }
        public string Rol { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaExpiracionOTP { get; set; }

        public string Correo { get; set; }

    }
}
