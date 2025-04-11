
using System;
using System.Collections.Generic;
using System;
using DTO;

namespace _00_DTO
{
    public class TransaccionCliente : BaseDTO
    {
        public double Monto { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }

     
        public int IdCliente { get; set; }  
    }
}
