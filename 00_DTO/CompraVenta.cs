using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace _00_DTO
{
    public class CompraVenta : BaseDTO
    {
        public int IdCliente { get; set; }
        public int IdAccion { get; set; }
        public int IdComision { get; set; }
        public double Cantidad { get; set; }
        public string Tipo { get; set; }
    }
}