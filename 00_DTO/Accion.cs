using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Accion : BaseDTO
    {
        public string? Nombre { get; set; }
        public string Simbolo { get; set; }
        public double PrecioActual { get; set; }
        public string? Mercado { get; set; }
        public string? Moneda { get; set; }
    }
}
