using System;
using System.Linq;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class CompraVentaManager
    {
        private readonly ComisionCrudFactory _comisionCrudFactory;
        private readonly CompraVentaCrudFactory _compraVentaCrudFactory;
        private readonly AccionCrudFactory _accionCrudFactory;

        public CompraVentaManager()
        {
            _comisionCrudFactory = new ComisionCrudFactory();
            _compraVentaCrudFactory = new CompraVentaCrudFactory();
            _accionCrudFactory = new AccionCrudFactory(); // Aseguramos que el CRUD de Acciones esté disponible
        }

        // Método para registrar una inversión
        public async Task RegistrarInversion(int idCliente, int idAccion, int cantidad, string tipo)
        {
            try
            {
                // 1. Obtener el precio actual de la acción
                double precioActual = ObtenerPrecioAccion(idAccion);

                // 2. Calcular el monto total de la inversión
                double montoTotal = precioActual * cantidad;

                // 3. Obtener todas las comisiones
                var comisiones = _comisionCrudFactory.RetrieveAll<Comision>();

                // 4. Buscar la comisión cuyo rango incluye el montoTotal
                var comisionSeleccionada = comisiones.FirstOrDefault(c =>
                    montoTotal >= c.MontoMin && montoTotal <= c.MontoMax
                );

                if (comisionSeleccionada == null)
                {
                    throw new Exception("No se encontró una comisión aplicable para el monto de la inversión.");
                }

                // 5. Crear objeto CompraVenta con la comisión asignada
                var compraVenta = new CompraVenta
                {
                    IdCliente = idCliente,
                    IdAccion = idAccion, // Se pasa el idAccion directamente
                    Cantidad = cantidad,
                    Tipo = tipo,
                    IdComision = comisionSeleccionada.Id
                };

                // 6. Registrar la inversión en la base de datos usando el CRUD
                _compraVentaCrudFactory.Create(compraVenta);

                // Opcional: Retornar el monto total de la inversión
                Console.WriteLine($"Inversión registrada con éxito. Monto Total: {montoTotal}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la inversión: {ex.Message}");
                throw;
            }
        }

        // Método para calcular la comisión aplicada basado en el precio actual y cantidad
        public double CalcularComision(double precioActual, double cantidad)
        {
            double montoTotal = precioActual * cantidad;
            var comisiones = _comisionCrudFactory.RetrieveAll<Comision>();
            double comisionAplicada = 0;

            foreach (var comision in comisiones)
            {
                if (montoTotal >= comision.MontoMin && montoTotal <= comision.MontoMax)
                {
                    // La comisión que corresponde
                    comisionAplicada = montoTotal * comision.Porcentaje / 100;
                    break;
                }
            }

            return comisionAplicada;
        }

        // Método para obtener el precio actual de la acción
        public double ObtenerPrecioAccion(int idAccion)
        {
            // Llamar al CRUD para obtener la acción por ID
            var accion = _accionCrudFactory.RetrieveById<Accion>(idAccion);

            // Verificar si la acción fue encontrada
            if (accion != null)
            {
                return accion.PrecioActual; // Retornar el precio actual de la acción
            }

            // Si no se encuentra la acción, lanzar una excepción
            throw new Exception("Acción no encontrada");
        }
    }
}



