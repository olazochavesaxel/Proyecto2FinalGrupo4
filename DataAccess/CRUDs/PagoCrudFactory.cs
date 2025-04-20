using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DataAccess.DAOs;
using _00_DTO;

namespace DataAccess.CRUDs
{
    public class PagoCrudFactory : CrudFactory
    {
        public PagoCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        // CREATE
        public override void Create(BaseDTO dto)
        {
            var pago = dto as Pago;

            var sqlOperation = new SqlOperation { ProcedureName = "CRE_PAGO_PR" };

            sqlOperation.AddDoubleParameter("MONTO", pago.Monto);
            sqlOperation.AddStringParameter("ESTADO", pago.Estado);
            sqlOperation.AddStringParameter("METODO", pago.Metodo);
            sqlOperation.AddDateTimeParameter("FECHA", pago.Fecha);
            sqlOperation.AddIntParameter("TRANSACCION_ID", pago.TransaccionId);
            sqlOperation.AddIntParameter("CLIENTE_ID", pago.ClienteId);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        // UPDATE
        public override void Update(BaseDTO dto)
        {
            var pago = dto as Pago;

            var sqlOperation = new SqlOperation { ProcedureName = "UPD_PAGO_PR" };

            sqlOperation.AddIntParameter("ID", pago.Id);
            sqlOperation.AddDoubleParameter("MONTO", pago.Monto);
            sqlOperation.AddStringParameter("ESTADO", pago.Estado);
            sqlOperation.AddStringParameter("METODO", pago.Metodo);
            sqlOperation.AddDateTimeParameter("FECHA", DateTime.Now);
            sqlOperation.AddIntParameter("TRANSACCION_ID", pago.TransaccionId);
            sqlOperation.AddIntParameter("CLIENTE_ID", pago.ClienteId);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        // DELETE
        public override void Delete(BaseDTO dto)
        {
            var pago = dto as Pago;

            var sqlOperation = new SqlOperation { ProcedureName = "DEL_PAGO_BY_ID_PR" };

            sqlOperation.AddIntParameter("ID", pago.Id);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        // RETRIEVE ALL
        public override List<T> RetrieveAll<T>()
        {
            var lstPagos = new List<T>();
            var sqlOperation = new SqlOperation { ProcedureName = "RETRIEVE_ALL_PAGOS_PR" };

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var pago = BuildPago(row);
                    lstPagos.Add((T)Convert.ChangeType(pago, typeof(T)));
                }
            }

            return lstPagos;
        }

        // RETRIEVE BY ID
        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation { ProcedureName = "RETRIEVE_PAGO_BY_ID_PR" };

            sqlOperation.AddIntParameter("ID", id);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var pago = BuildPago(row);
                return (T)Convert.ChangeType(pago, typeof(T));
            }

            return default(T);
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        // Helper para construir Pago desde diccionario de resultados
        private Pago BuildPago(Dictionary<string, object> row)
        {
            var pago = new Pago
            {
                Id = (int)row["id"],
                Monto = (double)row["monto"],
                Estado = (string)row["estado"],
                Metodo = (string)row["metodo"],
                Fecha = (DateTime)row["fecha"],
                TransaccionId = (int)row["transaccionCliente_Id"],
                ClienteId = (int)row["cliente_Id"]
            };

            return pago;
        }
    }
}
