using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Comision : BaseDTO
    {
        public string Tipo {get;set;}
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Porcentaje { get; set; }
    }
}
