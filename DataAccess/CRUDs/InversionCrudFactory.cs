using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class InversionCrudFactory : CrudFactory
    {
        public InversionCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            throw new NotImplementedException();
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
            var sqlOperation = new SqlOperation()
            {
                ProcedureName = "RETRIEVE_ALL_INVERSIONES"
            };

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            var inversiones = new List<T>();

            if (lstResults != null && lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var inversion = BuildInversion(row);
                    inversiones.Add((T)Convert.ChangeType(inversion, typeof(T)));
                }
            }

            return inversiones;
        }

        public override T RetrieveById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        private Inversion BuildInversion(Dictionary<string, object> row)
        {
            return new Inversion
            {
                Id = (int)row["id"],
                IntCliente = (int)row["intCliente"],
                IntAccion = (int)row["intAccion"],
                Cantidad = Convert.ToDouble(row["cantidad"]),
                Tipo = (string)row["tipo"],
                Created = (DateTime)row["fecha"],
                IdTransaccionCliente = (int)row["IdTransaccionCliente"],
                PrecioAccion = row["PrecioAccion"] != DBNull.Value ? (double?)Convert.ToDouble(row["PrecioAccion"]) : null
            };
        }
    }
}

