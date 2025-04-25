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
    public class PagoCrudFactory : CrudFactory

    {
        public PagoCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }
        private readonly string connectionString;

        public override void Create(BaseDTO dto)
        {
            var pago = dto as Pago;

            var sqlOperation = new SqlOperation { ProcedureName = "CRE_PAYPAL_TRANSACCION_PR" };

            sqlOperation.AddStringParameter("paypal_order_id", pago.PaypalOrderId);
            sqlOperation.AddStringParameter("payment_capture_id", pago.PaymentCaptureId);
            sqlOperation.AddStringParameter("payer_email", pago.PayerEmail);
            sqlOperation.AddStringParameter("payer_id", pago.PayerId);
            sqlOperation.AddDoubleParameter("amount", pago.Amount);
            sqlOperation.AddStringParameter("currency", pago.Currency);
            sqlOperation.AddStringParameter("status", pago.Status);



            _sqlDAO.ExecuteProcedure(sqlOperation);
        }





        public override void Delete(BaseDTO dto)
        {
            // Convertir DTO en Usuario
            var pago = dto as Pago;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_PAYPAL_TRANSACTION" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("id", pago.Id);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_PAYPAL_TRANSACTIONS_BY_ID" };
            sqlOperation.AddIntParameter("id", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildPaypalTransaction(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_PAYPAL_TRANSACTIONS" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var userObj = BuildPaypalTransaction(row);
                    lstUsers.Add((T)Convert.ChangeType(userObj, typeof(T)));
                }
            }

            return lstUsers;
        }


        public List<T> RetrieveByTransactionCliente<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_PAYPAL_TRANSACTION_BY_CLIENTE" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var userObj = BuildPaypalTransaction(row);
                    lstUsers.Add((T)Convert.ChangeType(userObj, typeof(T)));
                }
            }

            return lstUsers;
        }

        public List<T> RetrieveByTransactionAsesor<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_PAYPAL_TRANSACTION_BY_ASESOR" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var userObj = BuildPaypalTransaction(row);
                    lstUsers.Add((T)Convert.ChangeType(userObj, typeof(T)));
                }
            }

            return lstUsers;
        }

        private Pago BuildPaypalTransaction(Dictionary<string, object> row)
        {
            var pago = new Pago()
            {
                Id = row.ContainsKey("id") && row["id"] != DBNull.Value ? (int)row["id"] : 0,
                PaypalOrderId = row.ContainsKey("paypal_order_id") && row["paypal_order_id"] != DBNull.Value ? (string)row["paypal_order_id"] : string.Empty,
                PaymentCaptureId = row.ContainsKey("payment_capture_id") && row["payment_capture_id"] != DBNull.Value ? (string)row["payment_capture_id"] : string.Empty,
                PayerEmail = row.ContainsKey("payer_email") && row["payer_email"] != DBNull.Value ? (string)row["payer_email"] : string.Empty,
                PayerId = row.ContainsKey("payer_id") && row["payer_id"] != DBNull.Value ? (string)row["payer_id"] : string.Empty,
                Amount = row.ContainsKey("amount") && row["amount"] != DBNull.Value ? Convert.ToDouble(row["amount"]) : 0.0,
                Currency = row.ContainsKey("currency") && row["currency"] != DBNull.Value ? (string)row["currency"] : string.Empty,
                Status = row.ContainsKey("status") && row["status"] != DBNull.Value ? (string)row["status"] : string.Empty,
                Created = row.ContainsKey("create_time") && row["create_time"] != DBNull.Value ? (DateTime)row["create_time"] : DateTime.MinValue,
                UpdateTime = row.ContainsKey("update_time") && row["update_time"] != DBNull.Value ? (DateTime)row["update_time"] : DateTime.MinValue,

            };

            return pago;
        }

        public override void Update(BaseDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}