using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Transaccion :BaseDTO
    {
        public string PayPalOrderId { get; set; }
        public int UserId { get; set; }
        public string TipoOperacion { get; set; }
        public string Rol { get; set; }
    }
}
