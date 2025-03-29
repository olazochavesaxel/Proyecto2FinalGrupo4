using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Asesor : Usuario
    {
        public double IngresoComisiones { get; set; }
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}
