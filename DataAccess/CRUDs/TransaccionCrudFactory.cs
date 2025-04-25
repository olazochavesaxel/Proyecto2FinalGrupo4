using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using _00_DTO;
using DataAccess.DAOs;
using DTO;
using DTOs;

namespace DataAccess.CRUDs
{
    public class TransaccionCrudFactory : CrudFactory
    {
        public TransaccionCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        // Método para crear transacción de PayPal (rol ASESOR)
        public void Create_TR_PAYPAL_ASESOR(BaseDTO dto)
        {
            var trans = dto as Transaccion;

            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "CRE_INGRESO_EGRESO_PAYPAL_ASESOR"
            };

            sqlOperation.AddStringParameter("paypal_order_id", trans.PayPalOrderId);
            sqlOperation.AddIntParameter("userId", trans.UserId);
            sqlOperation.AddStringParameter("tipoOperacion", trans.TipoOperacion);

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        // Método para crear transacción de PayPal (rol CLIENTE)
        public void Create_TR_PAYPAL_CLIENTE(BaseDTO dto)
        {
            var trans = dto as Transaccion;

            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "CRE_INGRESO_EGRESO_PAYPAL_CLIENTE"
            };

            sqlOperation.AddStringParameter("paypal_order_id", trans.PayPalOrderId);
            sqlOperation.AddIntParameter("userId", trans.UserId);
            sqlOperation.AddStringParameter("tipoOperacion", trans.TipoOperacion);

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
            var lstTrans = new List<T>();
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "SELECT_TRANSACCIONES_CLIENTE"
            };

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var trans = BuildTransaccion(row);
                    lstTrans.Add((T)Convert.ChangeType(trans, typeof(T)));
                }
            }

            return lstTrans;
        }

        public override T RetrieveById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(BaseDTO dto)
        {
            throw new NotImplementedException();
        }
        private Transaccion BuildTransaccion(Dictionary<string, object> row)
        {
            var trans = new Transaccion()
            {
                Id = (int)row["Id"],
                PayPalOrderId = (string)(row["paypal_order_id"]),
                UserId = (int)row["userId"],
                TipoOperacion = (string)(row["tipoOperacion"]),
                Rol = (string)(row["rol"]) // Solo si este campo está en la BD
            };

            return trans;
        }



    }
}
