using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Accion : BaseDTO
    {
        public string Nombre { get; set; }
        public string Acronimo { get; set; }
        public double PrecioActual { get; set; }
       // public List<T> HistorialdePrecios { get; set; }
    }
}
