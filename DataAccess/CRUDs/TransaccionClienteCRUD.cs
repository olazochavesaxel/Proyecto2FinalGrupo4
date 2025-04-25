using System;
using System.Collections.Generic;
using DataAccess.DAOs;
using _00_DTO;
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

            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "CREAR_TRANSACCION_CLIENTE"
            };

            sqlOperation.AddDoubleParameter("Monto", trans.Monto);
            sqlOperation.AddDateTimeParameter("FechaCreacion", DateTime.Now);
            sqlOperation.AddStringParameter("Tipo", trans.Tipo);
            sqlOperation.AddStringParameter("Estado", "Activo");
            sqlOperation.AddIntParameter("IdCliente", trans.IdCliente);
            sqlOperation.AddIntParameter("idComision", trans.IdComision);
            sqlOperation.AddDoubleParameter("tarifaBaseAplicada", trans.TarifaBaseAplicada);
            sqlOperation.AddDoubleParameter("impuestoAplicado", trans.ImpuestoAplicado);
            sqlOperation.AddDoubleParameter("montoComision", trans.MontoComision);
            sqlOperation.AddStringParameter("reglaUsada", trans.ReglaUsada);
            sqlOperation.AddIntParameter("idAsesorEjecutor", trans.IdAsesorEjecutor);

            if (trans.Id_Paypal.HasValue)
                sqlOperation.AddIntParameter("IdPaypalTransaccion", trans.Id_Paypal.Value);
            else
                sqlOperation.AddNullParameter("IdPaypalTransaccion");

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            var trans = dto as TransaccionCliente;

            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "UDP_TRANSACCION_CLIENTE"
            };

            sqlOperation.AddIntParameter("Id", trans.Id);
            sqlOperation.AddDoubleParameter("Monto", trans.Monto);
            sqlOperation.AddDateTimeParameter("FechaCreacion", DateTime.Now);
            sqlOperation.AddStringParameter("Tipo", trans.Tipo);
            sqlOperation.AddStringParameter("Estado", "Activo");
            sqlOperation.AddIntParameter("IdCliente", trans.IdCliente);
            sqlOperation.AddIntParameter("idComision", trans.IdComision);
            sqlOperation.AddDoubleParameter("tarifaBaseAplicada", trans.TarifaBaseAplicada);
            sqlOperation.AddDoubleParameter("impuestoAplicado", trans.ImpuestoAplicado);
            sqlOperation.AddDoubleParameter("montoComision", trans.MontoComision);
            sqlOperation.AddStringParameter("reglaUsada", trans.ReglaUsada);
            sqlOperation.AddIntParameter("idAsesorEjecutor", trans.IdAsesorEjecutor);

            if (trans.Id_Paypal.HasValue)
                sqlOperation.AddIntParameter("IdPaypalTransaccion", trans.Id_Paypal.Value);
            else
                sqlOperation.AddNullParameter("IdPaypalTransaccion");

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO dto)
        {
            throw new NotImplementedException("Delete method not implemented for TransaccionCliente.");
        }

   

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACCION_CLIENTE_BY_ID" };
            sqlOperation.AddIntParameter("P_Id", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var trans = BuildTransaccionCliente(row);
                return (T)Convert.ChangeType(trans, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_TRANSACTION_CLIENTE_ALL_TRANSACTIONS"
            };


            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults == null || lstResults.Count == 0)
            {

                return new List<T>();
            }

            var transacciones = new List<T>();

            foreach (var row in lstResults)
            {
                var trans = BuildTransaccionCliente(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }

        public List<T> RetrieveByTipo<T>(string tipo)
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_TRANSACTION_CLIENTE_BY_TIPO"
            };

            sqlOperation.AddStringParameter("Tipo", tipo);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults == null || lstResults.Count == 0)
            {
                // Return an empty list if no results are found
                return new List<T>();
            }

            var transacciones = new List<T>();

            foreach (var row in lstResults)
            {
                var trans = BuildTransaccionCliente(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }


        public List<T> RetrieveByIdCliente<T>(int idCliente)
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_ALL_TRANSACTIONS_CLIENTE"
            };

            sqlOperation.AddIntParameter("IdCliente", idCliente);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults == null || lstResults.Count == 0)
            {
                // Return an empty list if no results are found
                return new List<T>();
            }

            var transacciones = new List<T>();

            foreach (var row in lstResults)
            {
                var trans = BuildTransaccionCliente(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }




        private TransaccionCliente BuildTransaccionCliente(Dictionary<string, object> row)
        {
            return new TransaccionCliente
            {
                Id = (int)row["Id"],
                Monto = row["Monto"] != DBNull.Value ? (double)row["Monto"] : 0.0,
                Created = row["FechaCreacion"] != DBNull.Value ? (DateTime)row["FechaCreacion"] : DateTime.MinValue,
                Tipo = row["Tipo"] != DBNull.Value ? (string)row["Tipo"] : null,
                IdCliente = row["IdCliente"] != DBNull.Value ? (int)row["IdCliente"] : 0,
                IdComision = row.ContainsKey("IdComision") && row["IdComision"] != DBNull.Value ? (int)row["IdComision"] : 0,
                TarifaBaseAplicada = (double)row["tarifaBaseAplicada"],
                ImpuestoAplicado = row["impuestoAplicado"] != DBNull.Value ? (double)row["impuestoAplicado"] : 0.0,
                MontoComision = row["montoComision"] != DBNull.Value ? (double)row["montoComision"] : 0.0,
                ReglaUsada = row["reglaUsada"] != DBNull.Value ? (string)row["reglaUsada"] : null,
                IdAsesorEjecutor = row["idAsesorEjecutor"] != DBNull.Value ? (int)row["idAsesorEjecutor"] : 0,
                Id_Paypal = row["IdPaypalTransaccion"] != DBNull.Value ? (int?)row["IdPaypalTransaccion"] : null
            };
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }
    }
}


