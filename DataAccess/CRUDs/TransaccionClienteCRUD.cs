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
    public class TransaccionClienteCrudFactory : CrudFactory
    {
        public TransaccionClienteCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            var trans = dto as TransaccionCliente;

            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_TRANSACCION_CLIENTE" };

            sqlOperation.AddDoubleParameter("P_MONTO", trans.Monto);
            sqlOperation.AddIntParameter("P_IDCLIENTE", trans.cliente.Id);

            sqlOperation.AddStringParameter("P_TIPO", trans.Tipo);
            sqlOperation.AddStringParameter("P_ESTADO", trans.Estado);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            var trans = dto as TransaccionCliente;

            var sqlOperation = new SqlOperation() { ProcedureName = "UDP_TRANSACCION_CLIENTE" };

            sqlOperation.AddIntParameter("P_ID", trans.Id);
            sqlOperation.AddDoubleParameter("P_MONTO", trans.Monto);
            sqlOperation.AddIntParameter("P_IDCLIENTE", trans.cliente.Id);
            sqlOperation.AddStringParameter("P_TIPO", trans.Tipo);
            sqlOperation.AddStringParameter("P_ESTADO", trans.Estado);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO dto)
        {
            var trans = dto as TransaccionCliente;

            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_TRANSACCION_CLIENTES" };

            sqlOperation.AddIntParameter("P_ID", trans.Id);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstTrans = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "SELECT_ALL_TRANSACCION_CLIENTE" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var trans = BuildTransaccionCliente(row);
                    lstTrans.Add((T)Convert.ChangeType(trans, typeof(T)));
                }
            }

            return lstTrans;
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACCIONES_CLIENTE_BY_ID" };
            sqlOperation.AddIntParameter("P_ID", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var trans = BuildTransaccionCliente(row);
                return (T)Convert.ChangeType(trans, typeof(T));
            }

            return default(T);
        }

        private TransaccionCliente BuildTransaccionCliente(Dictionary<string, object> row)
        {
            var trans = new TransaccionCliente()
            {
                Id = (int)row["Id"],
                Monto = Convert.ToSingle(row["Monto"]),
                Created = (DateTime)row["FechaCreacion"],
                Tipo = row["Tipo"].ToString(),
                Estado = row["Estado"].ToString()
            };

            return trans;
        }
    }
}

