using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.CRUDs;
using DTO;

namespace CoreApp
{
    public class PagoManager
    {
        private readonly PagoCrudFactory crud;

        public PagoManager()
        {
            crud = new PagoCrudFactory();
        }

        // Registrar un nuevo pago
        public void RegistrarPago(Pago pago)
        {
            crud.Create(pago);
        }

        // Actualizar un pago existente
        public void ActualizarPago(Pago pago)
        {
            crud.Update(pago);
        }

        // Eliminar un pago
        public void EliminarPago(int id)
        {
            var pago = new Pago { Id = id };
            crud.Delete(pago);
        }

        // Obtener todos los pagos
        public List<Pago> ObtenerTodos()
        {
            return crud.RetrieveAll<Pago>();
        }

        // Obtener pago por ID
        public Pago ObtenerPorId(int id)
        {
            return crud.RetrieveById<Pago>(id);
        }
    }
}
