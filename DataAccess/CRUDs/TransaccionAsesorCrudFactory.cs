using _00_DTO;
using DataAccess.DAOs;
using DTO;
using DTOs;

using TransaccionAsesor = DTOs.TransaccionAsesorDTO;



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

            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_TRANSACCION_ASESOR" };
            sqlOperation.AddDoubleParameter("P_MONTO", trans.Monto);
            sqlOperation.AddIntParameter("P_IDASESOR", trans.IdAsesor);
            sqlOperation.AddIntParameter("P_IDCLIENTE", trans.IdCliente);
            sqlOperation.AddStringParameter("P_TIPO", trans.Tipo);
            sqlOperation.AddStringParameter("P_ESTADO", trans.Estado);
            sqlOperation.AddDateTimeParameter("P_CREATED", trans.Created.Value);



            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            var trans = dto as TransaccionAsesor;

            var sqlOperation = new SqlOperation() { ProcedureName = "UDP_TRANSACCION_ASESOR" };
            sqlOperation.AddIntParameter("P_ID", trans.Id);
            sqlOperation.AddDoubleParameter("P_MONTO", trans.Monto);
            sqlOperation.AddIntParameter("P_IDASESOR", trans.IdAsesor);
            sqlOperation.AddIntParameter("P_IDCLIENTE", trans.IdCliente);
            sqlOperation.AddStringParameter("P_TIPO", trans.Tipo);
            sqlOperation.AddStringParameter("P_ESTADO", trans.Estado);


            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO dto)
        {
            var trans = dto as TransaccionAsesor;

            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_TRANSACCION_ASESOR" };
            sqlOperation.AddIntParameter("P_ID", trans.Id);
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override List<T> RetrieveAll<T>()
        {
            var lst = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "SELECT_ALL_TRANSACCION_ASESOR" };
            var results = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in results)
            {
                var item = Build(row);
                lst.Add((T)Convert.ChangeType(item, typeof(T)));
            }

            return lst;
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_TRANSACCION_ASESOR_BY_ID" };
            sqlOperation.AddIntParameter("P_ID", id);
            var result = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (result.Count > 0)
            {
                var item = Build(result[0]);
                return (T)Convert.ChangeType(item, typeof(T));
            }

            return default(T);
        }

        public override T Retrieve<T>() => throw new NotImplementedException();

        private TransaccionAsesor Build(Dictionary<string, object> row)
        {
            return new TransaccionAsesor
            {
                Id = (int)row["Id"],
                Monto = Convert.ToDouble(row["Monto"]),

                Tipo = row["Tipo"].ToString(),
                Estado = row["Estado"].ToString(),
                IdAsesor = (int)row["IdAsesor"],
                IdCliente = (int)row["IdCliente"],
                Created = row["Created"] != DBNull.Value ? (DateTime)row["Created"] : (DateTime?)null



            };
        }
    }
}



