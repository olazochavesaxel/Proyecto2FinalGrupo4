using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    internal class ComisionManager
    {
        private double tarifa1; // Tarifa para montos bajos
        private double tarifa2; // Tarifa para montos medios
        private double tarifa3; // Tarifa para montos altos
        private readonly double porcentajeGeneral = 2.0; // Porcentaje para montos muy altos

        public ComisionManager(double tarifa1, double tarifa2, double tarifa3)
        {
            this.tarifa1 = tarifa1;
            this.tarifa2 = tarifa2;
            this.tarifa3 = tarifa3;
        }

        public void ActualizarTarifas(double nuevaTarifa1, double nuevaTarifa2, double nuevaTarifa3)
        {
            tarifa1 = nuevaTarifa1;
            tarifa2 = nuevaTarifa2;
            tarifa3 = nuevaTarifa3;
        }

        public double CalcularComisionCliente(double monto, string tipo)
        {
            if (monto > 5000)
            {
                return monto * (porcentajeGeneral / 100);
            }
            else if (monto > 1000)
            {
                return tarifa3;
            }
            else if (monto > 500)
            {
                return tarifa2;
            }
            else
            {
                return tarifa1;
            }
        }

        public double CalcularComisionAsesor(double monto, bool conGanancia)
        {
            return conGanancia ? monto * 0.02 : monto * 0.01;
        }
    }
}

