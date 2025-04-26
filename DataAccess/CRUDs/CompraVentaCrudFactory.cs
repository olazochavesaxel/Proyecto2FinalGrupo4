using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _00_DTO;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class CompraVentaCrudFactory : CrudFactory
    {
        public CompraVentaCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        // Método para registrar la compra o venta
        public override void Create(BaseDTO dto)
        {
            var compraVenta = dto as CompraVenta;

            // Crear instrucción de ejecución para el procedimiento almacenado
            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_REGISTRAR_INVERSION_CON_LOGICA" };

            // Agregar parámetros al procedimiento almacenado para la inversión
            sqlOperation.AddIntParameter("@IdCliente", compraVenta.IdCliente);
            sqlOperation.AddIntParameter("@IdAccion", compraVenta.IdAccion);
            sqlOperation.AddDoubleParameter("@Cantidad", compraVenta.Cantidad);
            sqlOperation.AddStringParameter("@Tipo", compraVenta.Tipo); // 'Compra' o 'Venta'
            sqlOperation.AddIntParameter("@IdComision", compraVenta.IdComision);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            throw new NotImplementedException();
        }

        public override T RetrieveById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(BaseDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
