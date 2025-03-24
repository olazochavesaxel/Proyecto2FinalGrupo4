using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Transaccion :BaseDTO
    {
        public Usuario usuario;
        public double Monto { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }//Aprovado, Rechazado o Denegado
    }
}
