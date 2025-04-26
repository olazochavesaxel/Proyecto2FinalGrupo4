using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Inversion : BaseDTO
    {


        public int IntCliente { get; set; }
        public int IntAccion { get; set; }
        public double Cantidad { get; set; }
        public string Tipo { get; set; }

        public int IdTransaccionCliente { get; set; }
        public double? PrecioAccion { get; set; }
    }
}

