using System;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class ClienteCrudFactory : CrudFactory
    {
        public ClienteCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            var cliente = dto as Cliente;

            // Crear instrucción de ejecución para crear usuario
            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_CLIENTE_PR" };

            // Agregar parámetros al procedimiento almacenado para tblUsuario
            sqlOperation.AddStringParameter("P_CEDULA", cliente.Cedula);
            sqlOperation.AddStringParameter("P_NOMBRE", cliente.Nombre);
            sqlOperation.AddStringParameter("P_PRIMER_APELLIDO", cliente.PrimerApellido);
            sqlOperation.AddStringParameter("P_SEGUNDO_APELLIDO", cliente.SegundoApellido);
            sqlOperation.AddStringParameter("P_DIRECCION", cliente.Direccion);
            sqlOperation.AddStringParameter("P_FOTO_PERFIL", cliente.FotoPerfil);
            sqlOperation.AddStringParameter("P_CONTRASENNA", cliente.Contrasenna);
            sqlOperation.AddStringParameter("P_TELEFONO", cliente.Telefono);
            sqlOperation.AddStringParameter("P_ESTADO", cliente.Estado);
            sqlOperation.AddStringParameter("P_ROL", "Cliente");
            sqlOperation.AddDateTimeParameter("P_FECHA_NACIMIENTO", cliente.FechaNacimiento);
            sqlOperation.AddDateTimeParameter("P_FECHA_EXPIRACION_OTP", cliente.FechaExpiracionOTP);
            sqlOperation.AddStringParameter("P_CORREO", cliente.Correo);  // Agregar correo
            sqlOperation.AddDoubleParameter("P_BALANCE_FINANCIERO", cliente.BalanceFinanciero);  // Balance financiero

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            var cliente = dto as Cliente;

            // Crear instrucción de ejecución para actualizar usuario
            var sqlOperation = new SqlOperation() { ProcedureName = "UDP_CLIENTE_PR" };

            // Agregar parámetros al procedimiento almacenado para tblUsuario y tblCliente
            sqlOperation.AddIntParameter("P_ID", cliente.Id);
            sqlOperation.AddStringParameter("P_CEDULA", cliente.Cedula);
            sqlOperation.AddStringParameter("P_NOMBRE", cliente.Nombre);
            sqlOperation.AddStringParameter("P_PRIMER_APELLIDO", cliente.PrimerApellido);
            sqlOperation.AddStringParameter("P_SEGUNDO_APELLIDO", cliente.SegundoApellido);
            sqlOperation.AddStringParameter("P_DIRECCION", cliente.Direccion);
            sqlOperation.AddStringParameter("P_FOTO_PERFIL", cliente.FotoPerfil);
            sqlOperation.AddStringParameter("P_CONTRASENNA", cliente.Contrasenna);
            sqlOperation.AddStringParameter("P_TELEFONO", cliente.Telefono);
            sqlOperation.AddStringParameter("P_ESTADO", cliente.Estado);
            sqlOperation.AddStringParameter("P_ROL", "Cliente");
            sqlOperation.AddDateTimeParameter("P_FECHA_NACIMIENTO", cliente.FechaNacimiento);

            sqlOperation.AddDateTimeParameter("P_FECHA_EXPIRACION_OTP", cliente.FechaExpiracionOTP);
            sqlOperation.AddStringParameter("P_CORREO", cliente.Correo);  // Agregar correo
            sqlOperation.AddDoubleParameter("P_BALANCE_FINANCIERO", cliente.BalanceFinanciero);  // Balance financiero

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO dto)
        {
            var cliente = dto as Cliente;

            // Crear instrucción de ejecución para eliminar usuario
            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_CLIENTE_BY_ID" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("P_ID", cliente.Id);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }
        public override List<T> RetrieveAll<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_CLIENTES" };
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var userObj = BuildUser(row);
                    lstUsers.Add((T)Convert.ChangeType(userObj, typeof(T)));
                }
            }

            return lstUsers;
        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_CLIENTE_BY_ID" };
            sqlOperation.AddIntParameter("P_ID", id);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildUser(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByCedula<T>(string Cedula)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_CLIENTE_BY_CEDULA" };
            sqlOperation.AddStringParameter("P_CEDULA", Cedula);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildUser(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByCorreo<T>(string Correo)
        {
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_CLIENTE_BY_CORREO" };
            sqlOperation.AddStringParameter("P_CORREO", Correo);
            var lstResults = _sqlDAO.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                var user = BuildUser(row);
                return (T)Convert.ChangeType(user, typeof(T));
            }

            return default(T);
        }

        // Convierte un diccionario en un DTO Usuario
        private Cliente BuildUser(Dictionary<string, object> row)
        {
            var newUser = new Cliente()
            {
                Id = (int)row["id"],
                Cedula = (string)row["Cedula"],
                Nombre = (string)row["Nombre"],
                PrimerApellido = (string)row["PrimerApellido"],
                SegundoApellido = (string)row["SegundoApellido"],
                Direccion = (string)row["Direccion"],
                FotoPerfil = (string)row["FotoPerfil"],
                Contrasenna = (string)row["Contrasenna"],
                Telefono = (string)row["Telefono"],
                Estado = (string)row["Estado"],
                Rol = (string)row["Rol"],
                FechaNacimiento = (DateTime)row["FechaNacimiento"],
                Correo= (string)row["Correo"],
                FechaExpiracionOTP = (DateTime)row["FechaExpiracionOTP"],
                Created = (DateTime)row["FechaCreacion"],
                BalanceFinanciero = (double)row["BalanceFinanciero"],
                Correo = (string)row["Correo"]
            };

            return newUser;
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }
    }
}
