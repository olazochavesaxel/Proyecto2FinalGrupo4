using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class AdminCrudFactory : CrudFactory
    {
        public AdminCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            var user = dto as Admin;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_ADMIN_PR" };

            // Agregar parámetros al procedimiento almacenado

            sqlOperation.AddStringParameter("P_CEDULA", user.Cedula);
            sqlOperation.AddStringParameter("P_NOMBRE", user.Nombre);
            sqlOperation.AddStringParameter("P_PRIMER_APELLIDO", user.PrimerApellido);
            sqlOperation.AddStringParameter("P_SEGUNDO_APELLIDO", user.SegundoApellido);
            sqlOperation.AddStringParameter("P_DIRECCION", user.Direccion);
            sqlOperation.AddStringParameter("P_FOTO_PERFIL", user.FotoPerfil);
            sqlOperation.AddStringParameter("P_CONTRASENNA", user.Contrasenna);
            sqlOperation.AddStringParameter("P_TELEFONO", user.Telefono);
            sqlOperation.AddStringParameter("P_ESTADO", "Activo");
            sqlOperation.AddStringParameter("P_ROL", "Admin");
            sqlOperation.AddDateTimeParameter("P_FECHA_NACIMIENTO", user.FechaNacimiento);
            sqlOperation.AddDateTimeParameter("P_FECHA_EXPIRACION_OTP", user.FechaExpiracionOTP);
            sqlOperation.AddStringParameter("P_CORREO", user.Correo);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            // Convertir DTO en Usuario
            var user = dto as Admin;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "UDP_ADMIN_PR" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("P_ID", user.Id);
            sqlOperation.AddStringParameter("P_CEDULA", user.Cedula);
            sqlOperation.AddStringParameter("P_NOMBRE", user.Nombre);
            sqlOperation.AddStringParameter("P_PRIMER_APELLIDO", user.PrimerApellido);
            sqlOperation.AddStringParameter("P_SEGUNDO_APELLIDO", user.SegundoApellido);
            sqlOperation.AddStringParameter("P_DIRECCION", user.Direccion);
            sqlOperation.AddStringParameter("P_FOTO_PERFIL", user.FotoPerfil);
            sqlOperation.AddStringParameter("P_CONTRASENNA", user.Contrasenna);
            sqlOperation.AddStringParameter("P_TELEFONO", user.Telefono);
            sqlOperation.AddStringParameter("P_ESTADO", user.Estado);
            sqlOperation.AddStringParameter("P_ROL", "Admin");
            sqlOperation.AddDateTimeParameter("P_FECHA_NACIMIENTO", user.FechaNacimiento);
            sqlOperation.AddDateTimeParameter("P_FECHA_EXPIRACION_OTP", user.FechaExpiracionOTP);
            sqlOperation.AddStringParameter("P_CORREO", user.Correo);


            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO dto)
        {
            // Convertir DTO en Usuario
            var user = dto as Admin;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_ADMIN_BY_ID" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("P_ID", user.Id);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }
        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_ADMINS" };
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
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ADMIN_BY_ID" };
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
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ADMIN_BY_CEDULA" };
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
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ADMIN_BY_CORREO" };
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

        private Admin BuildUser(Dictionary<string, object> row)
        {
            var newUser = new Admin()
            {
                // Asignar directamente los valores, sin valor predeterminado
                Id = (int)row["id"], // Aquí usamos default(int?) para permitir que el valor sea null
                Cedula = row.ContainsKey("Cedula") && row["Cedula"] != DBNull.Value ? (string)row["Cedula"] : string.Empty,
                Nombre = row.ContainsKey("Nombre") && row["Nombre"] != DBNull.Value ? (string)row["Nombre"] : string.Empty,
                PrimerApellido = row.ContainsKey("PrimerApellido") && row["PrimerApellido"] != DBNull.Value ? (string)row["PrimerApellido"] : string.Empty,
                SegundoApellido = row.ContainsKey("SegundoApellido") && row["SegundoApellido"] != DBNull.Value ? (string)row["SegundoApellido"] : string.Empty,
                Direccion = row.ContainsKey("Direccion") && row["Direccion"] != DBNull.Value ? (string)row["Direccion"] : string.Empty,
                FotoPerfil = row.ContainsKey("FotoPerfil") && row["FotoPerfil"] != DBNull.Value ? (string)row["FotoPerfil"] : string.Empty,
                Telefono = row.ContainsKey("Telefono") && row["Telefono"] != DBNull.Value ? (string)row["Telefono"] : string.Empty,
                Estado = row.ContainsKey("Estado") && row["Estado"] != DBNull.Value ? (string)row["Estado"] : string.Empty,
                Contrasenna = row.ContainsKey("Contrasenna") && row["Contrasenna"] != DBNull.Value ? (string)row["Contrasenna"] : string.Empty,
                Rol = row.ContainsKey("Rol") && row["Rol"] != DBNull.Value ? (string)row["Rol"] : string.Empty,
                FechaNacimiento = row.ContainsKey("FechaNacimiento") && row["FechaNacimiento"] != DBNull.Value ? (DateTime)row["FechaNacimiento"] : DateTime.MinValue,
                FechaExpiracionOTP = row.ContainsKey("FechaExpiracionOTP") && row["FechaExpiracionOTP"] != DBNull.Value ? (DateTime)row["FechaExpiracionOTP"] : DateTime.MinValue,
                Created = row.ContainsKey("FechaCreacion") && row["FechaCreacion"] != DBNull.Value ? (DateTime)row["FechaCreacion"] : DateTime.MinValue,
                Correo = row.ContainsKey("Correo") && row["Correo"] != DBNull.Value ? (string)row["Correo"] : string.Empty,
            };

            return newUser;
        }


      
    }
}