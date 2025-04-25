using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace _00_DTO
{
    public class IngresoPlataforma :BaseDTO
    {
        public double MontoIngreso { get; set; }        
        public string Descripcion { get; set; }          
        public int IdCliente { get; set; }             
        public int? IdAsesor { get; set; }              
        public int IdComision { get; set; }
        public DateTime? FechaFinalFiltro { get; set; }

    }
}
