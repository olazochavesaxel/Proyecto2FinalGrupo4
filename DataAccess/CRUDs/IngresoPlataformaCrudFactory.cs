using System;
using System.Collections.Generic;
using _00_DTO;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class IngresoPlataformaCrudFactory : CrudFactory
    {
        public IngresoPlataformaCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_INGRESOS" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var userObj = BuildIngresoPlataforma2(row);
                    lstUsers.Add((T)Convert.ChangeType(userObj, typeof(T)));
                }
            }

            return lstUsers;
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACCION_CLIENTE_BY_ID" };
            sqlOperation.AddIntParameter("P_Id", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var trans = BuildIngresoPlataforma(row);
                return (T)Convert.ChangeType(trans, typeof(T));
            }

            return default(T);
        }

        public List<T> RetrieveByFechaIngreso<T>(IngresoPlataforma filtro) where T : class
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_INGRESO_PLATAFORMA_BY_FECHA" };

            // Use Create instead of FechaIngreso
            sqlOperation.AddDateTimeParameter("@FechaInicio", filtro.Created);
            sqlOperation.AddDateTimeParameter("@FechaFin", filtro.FechaFinalFiltro ?? DateTime.Now);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults == null || lstResults.Count == 0)
            {
                return new List<T>();
            }

            var resultList = new List<T>();

            foreach (var row in lstResults)
            {
                // Use a method that converts each row into the expected object
                var ingreso = BuildIngresoPlataforma(row);

                // Ensure that T is compatible with the expected result type
                if (typeof(T) == typeof(IngresoPlataforma))
                {
                    resultList.Add(ingreso as T);
                }
                else
                {
                    // Here you would handle cases where T is not exactly IngresoPlataforma
                    // You could do additional mapping or throw an exception if the types are incompatible
                    // For this example, I'm assuming you just want to return an empty list if types don't match
                    return new List<T>();
                }
            }

            return resultList;
        }



        public List<T> RetrieveByIdAsesor<T>(int idAsesor)
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_PLATAFORMA_BY_ID_ASESOR"
            };

            sqlOperation.AddIntParameter("IdAsesor", idAsesor);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults == null || lstResults.Count == 0)
            {
                return new List<T>();
            }

            var transacciones = new List<T>();

            foreach (var row in lstResults)
            {
                var trans = BuildIngresoPlataforma(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }

        public List<T> RetrieveByIdCliente<T>(int idCliente)
        {
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_PLATAFORMA_BY_ID_CLIENTE"
            };

            sqlOperation.AddIntParameter("IdCliente", idCliente);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults == null || lstResults.Count == 0)
            {
                return new List<T>();
            }

            var transacciones = new List<T>();

            foreach (var row in lstResults)
            {
                var trans = BuildIngresoPlataforma(row);
                transacciones.Add((T)Convert.ChangeType(trans, typeof(T)));
            }

            return transacciones;
        }

        private IngresoPlataforma BuildIngresoPlataforma(Dictionary<string, object> row)
        {
            return new IngresoPlataforma
            {
                Id = row.ContainsKey("Id") && row["Id"] != DBNull.Value ? (int)row["Id"] : 0,
                Created = row.ContainsKey("Create") && row["Create"] != DBNull.Value ? (DateTime)row["Create"] : DateTime.MinValue, // Cambiar a "Create"
                MontoIngreso = row.ContainsKey("MontoIngreso") && row["MontoIngreso"] != DBNull.Value ? (double)row["MontoIngreso"] : 0.0,
                Descripcion = row.ContainsKey("Descripcion") && row["Descripcion"] != DBNull.Value ? (string)row["Descripcion"] : null,
                IdCliente = row.ContainsKey("IdCliente") && row["IdCliente"] != DBNull.Value ? (int)row["IdCliente"] : 0,
                IdAsesor = row.ContainsKey("IdAsesor") && row["IdAsesor"] != DBNull.Value ? (int?)row["IdAsesor"] : null,
                IdComision = row.ContainsKey("IdComision") && row["IdComision"] != DBNull.Value ? (int)row["IdComision"] : 0
            };
        }
        private IngresoPlataforma BuildIngresoPlataforma2(Dictionary<string, object> row)
        {
            return new IngresoPlataforma
            {
                Id = row.ContainsKey("id") && row["id"] != DBNull.Value ? (int)row["id"] : 0,
                Created = row.ContainsKey("Create") && row["Create"] != DBNull.Value ? (DateTime)row["Create"] : DateTime.MinValue, // Cambiar a "Create"
                MontoIngreso = row.ContainsKey("MontoIngreso") && row["MontoIngreso"] != DBNull.Value ? (double)row["MontoIngreso"] : 0.0,
                Descripcion = row.ContainsKey("Descripcion") && row["Descripcion"] != DBNull.Value ? (string)row["Descripcion"] : null,
                IdCliente = row.ContainsKey("IdCliente") && row["IdCliente"] != DBNull.Value ? (int)row["IdCliente"] : 0,
                IdAsesor = row.ContainsKey("IdAsesor") && row["IdAsesor"] != DBNull.Value ? (int?)row["IdAsesor"] : null,
                IdComision = row.ContainsKey("IdComision") && row["IdComision"] != DBNull.Value ? (int)row["IdComision"] : 0
            };
        }

        // Implementación de métodos para Insertar, Actualizar y Eliminar
        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override void Create(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        public override void Update(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        public override void Delete(BaseDTO dto)
        {
            throw new NotImplementedException();
        }


    




    }
}



