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

            // Crear instrucción de ejecución para crear usuario

            var sqlOperation = new SqlOperation { ProcedureName = "CRE_PAGO_PR" };

            // Agregar parámetros al procedimiento almacenado para tblUsuario


            sqlOperation.AddDoubleParameter("MONTO", pago.Monto);
            sqlOperation.AddStringParameter("ESTADO", pago.Estado);
            sqlOperation.AddStringParameter("METODO", pago.Metodo);
            sqlOperation.AddDateTimeParameter("FECHA", pago.Fecha);
            sqlOperation.AddIntParameter("TRANSACCION_ID", pago.TransaccionId);
            sqlOperation.AddIntParameter("CLIENTE_ID", pago.ClienteId);



            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
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

        public override T RetrieveById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}