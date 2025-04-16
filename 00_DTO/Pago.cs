using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _00_DTO
{
    public class Pago
    {
        public int Id { get; set; }
        public double Monto { get; set; }
        public string Estado { get; set; } // Aprobado, Pendiente, Cancelado
        public string Metodo { get; set; } // "PayPal"
        public DateTime Fecha { get; set; }
        public int TransaccionId { get; set; } // ID de PayPal
        public int ClienteId { get; set; }
    }

}
