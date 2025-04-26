using _00_DTO;
using DataAccess.DAOs;
using DTO;
using DTOs;
using System;
using System.Collections.Generic;

// Alias útil si hay colisión de nombres
using TransaccionAsesor = DTOs.TransaccionAsesor;

namespace DataAccess.CRUDs
{
    public class TransaccionAsesorCrudFactory : CrudFactory
    {
        public TransaccionAsesorCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            var trans = dto as TransaccionAsesor;

            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "CRE_TRANSACCION_ASESOR"
            };

            if (trans.Monto.HasValue)
                sqlOperation.AddDoubleParameter("P_MONTO", trans.Monto.Value);
            else
                sqlOperation.AddNullParameter("P_MONTO");

            if (trans.IdAsesor.HasValue)
                sqlOperation.AddIntParameter("P_IDASESOR", trans.IdAsesor.Value);
            else
                sqlOperation.AddNullParameter("P_IDASESOR");

            if (trans.IdCliente.HasValue)
                sqlOperation.AddIntParameter("P_IDCLIENTE", trans.IdCliente.Value);
            else
                sqlOperation.AddNullParameter("P_IDCLIENTE");

            sqlOperation.AddStringParameter("P_TIPO", trans.Tipo ?? "");     // Si puede ser null, considerar AddNullParameter
            sqlOperation.AddStringParameter("P_ESTADO", trans.Estado ?? ""); // Lo mismo aquí

            if (trans.Id_Paypal.HasValue)
                sqlOperation.AddIntParameter("P_IDPAYPALTRANSACCION", trans.Id_Paypal.Value);
            else
                sqlOperation.AddNullParameter("P_IDPAYPALTRANSACCION");

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }


        public override void Update(BaseDTO dto)
        {
            var trans = dto as TransaccionAsesor;

            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "UDP_TRANSACCION_ASESOR"
            };

            sqlOperation.AddIntParameter("Id", trans.Id);

            if (trans.Monto.HasValue)
                sqlOperation.AddDoubleParameter("Monto", trans.Monto.Value);
            else
                sqlOperation.AddNullParameter("Monto");

            sqlOperation.AddStringParameter("Tipo", trans.Tipo ?? "");
            sqlOperation.AddStringParameter("Estado", trans.Estado ?? "");

            if (trans.IdAsesor.HasValue)
                sqlOperation.AddIntParameter("IdAsesor", trans.IdAsesor.Value);
            else
                sqlOperation.AddNullParameter("IdAsesor");

            if (trans.IdCliente.HasValue)
                sqlOperation.AddIntParameter("IdCliente", trans.IdCliente.Value);
            else
                sqlOperation.AddNullParameter("IdCliente");

            if (trans.Id_Paypal.HasValue)
                sqlOperation.AddIntParameter("IdPaypalTransaccion", trans.Id_Paypal.Value);
            else
                sqlOperation.AddNullParameter("IdPaypalTransaccion");

            _sqlDAO.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO dto)
        {
            var trans = dto as TransaccionAsesor;
            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_TRANSACCION_ASESOR" };
            sqlOperation.AddIntParameter("Id", trans.Id);
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstTrans = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_TRANSACTIONS_ASESOR" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var trans = BuildTransaccionAsesor(row);
                lstTrans.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return lstTrans;
        }
        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACTION_ASESOR_BY_ID_UNICO" };
            sqlOperation.AddIntParameter("ID", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildTransaccionAsesor(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }

        public List<T> RetrieveByIdAsesor<T>(int id)
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_TRANSACCIONES_ASESOR_BY_ID"
            };

            sqlOperation.AddIntParameter("IdAsesor", id);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            var transacciones = new List<T>();

            foreach (var row in lstResults)
            {
                var trans = BuildTransaccionAsesor(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }





        public T RetrieveByPaypal<T>(int idComision)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACTION_ASESOR_BY_COMISION" };
            sqlOperation.AddIntParameter("IdComision", idComision);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var trans = BuildTransaccionAsesor(row);
                return (T)Convert.ChangeType(trans, typeof(T));
            }

            return default(T);
        }

        public List<T> RetrieveByTipo<T>(List<string> tipos)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACTION_ASESOR_BY_TIPO" };

            // Convertimos la lista de tipos a una cadena separada por comas
            string tiposConcatenados = string.Join(",", tipos);
            sqlOperation.AddStringParameter("Tipo", tiposConcatenados);

            // Ejecutamos la consulta SQL y obtenemos los resultados
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            // Si tenemos resultados, los convertimos al tipo T y los devolvemos
            List<T> lstTrans = new List<T>();

            foreach (var row in lstResults)
            {
                // Convertimos cada resultado a TransaccionAsesor y luego a T
                var trans = BuildTransaccionAsesor(row); // Asumimos que BuildTransaccionAsesor devuelve un TransaccionAsesor
                lstTrans.Add((T)Convert.ChangeType(trans, typeof(T))); // Convertimos el objeto al tipo T
            }

            return lstTrans; // Devolvemos la lista de resultados convertidos
        }



        public List<T> RetrieveByMyClientes<T>(int idCliente)
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVETRANSACCION_ASESOR__ALL_CLIENTES"
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
                var trans = BuildTransaccionAsesor(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }



        private TransaccionAsesor BuildTransaccionAsesor(Dictionary<string, object> row)
        {
            return new TransaccionAsesor
            {
                Id = (int)row["Id"],
                Monto = row["Monto"] != DBNull.Value ? (double)row["Monto"] : 0.0,
                IdAsesor = row["IdAsesor"] != DBNull.Value ? (int)row["IdAsesor"] : 0,
                IdCliente = row["IdCliente"] != DBNull.Value ? (int)row["IdCliente"] : 0,
                Tipo = row["Tipo"] != DBNull.Value ? (string)row["Tipo"] : null,
                Estado = row["Estado"] != DBNull.Value ? (string)row["Estado"] : null,
                Created = row["Created"] != DBNull.Value ? (DateTime)row["Created"] : DateTime.MinValue,
                Id_Paypal = row["IdPaypalTransaccion"] != DBNull.Value ? (int?)row["IdPaypalTransaccion"] : null
            };
        }



        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

       
    }
}
