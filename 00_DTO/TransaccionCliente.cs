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
        public int IdComision { get; set; }
        public double TarifaBaseAplicada { get; set; }
        public double ImpuestoAplicado { get; set; }
        public double MontoComision { get; set; }
        public string ReglaUsada { get; set; }
        public int IdAsesorEjecutor { get; set; }
        public int? Id_Paypal { get; set; }
    }
}
