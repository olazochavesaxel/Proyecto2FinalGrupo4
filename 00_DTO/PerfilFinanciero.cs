using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PerfilFinanciero : BaseDTO
    {
        public Cliente client;
      public string riesgo { get; set; }
      public List<Inversion>  inversionesPrevias;
      public string Preferencias { get; set; }

    }
}
