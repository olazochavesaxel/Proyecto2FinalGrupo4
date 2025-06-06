﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Comision : BaseDTO
    {
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public double Porcentaje { get; set; }
        public double Tarifa1 { get; set; }
        public double Tarifa2 { get; set; }
        public double Tarifa3 { get; set; }
        public int idAdmin { get; set; }
        public double MontoMin {  get; set; }

        public double MontoMax { get; set; }

        public double PorcentajeAsesor { get; set; }
    }
}
