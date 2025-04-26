using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class AccionCrudFactory : CrudFactory
    {
        public AccionCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        public void CreateUpdate(BaseDTO dto)
        {
            var accion = dto as Accion;

            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_REGISTRAR_O_ACTUALIZAR_ACCION" };

            sqlOperation.AddStringParameter("Nombre", accion.Nombre);
            sqlOperation.AddStringParameter("Simbolo", accion.Simbolo);
            sqlOperation.AddDoubleParameter("PrecioActual", accion.PrecioActual);
            sqlOperation.AddStringParameter("Mercado", accion.Mercado);
            sqlOperation.AddStringParameter("Moneda", accion.Moneda);

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
            var lstAcciones = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_ACCIONES" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var accionObj = BuildAccion(row);
                    lstAcciones.Add((T)Convert.ChangeType(accionObj, typeof(T)));
                }
            }

            return lstAcciones;
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ACTION_BY_ID" };
            sqlOperation.AddIntParameter("P_ID", id);

            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var accionObj = BuildAccion(row);
                return (T)Convert.ChangeType(accionObj, typeof(T));
            }

            return default(T);
        }

        public override void Update(BaseDTO dto)
        {
            throw new NotImplementedException();
        }

        private Accion BuildAccion(Dictionary<string, object> row)
        {
            return new Accion
            {
                Id = (int)row["id"],
                Nombre = (string)row["nombre"],
                Simbolo = (string)row["simbolo"],
                PrecioActual = (double)row["precioActual"],
                Created = (DateTime)row["fechaActualizacion"],
                Mercado = (string)row["mercado"],
                Moneda = (string)row["moneda"]
            };
        }
    }
}
