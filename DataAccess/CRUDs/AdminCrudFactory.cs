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


        // Convierte un diccionario en un DTO Usuario
        private Admin BuildUser(Dictionary<string, object> row)
        {
            var newUser = new Admin()
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
                FechaExpiracionOTP = (DateTime)row["FechaExpiracionOTP"],
                Created = (DateTime)row["FechaCreacion"],
                Correo = (string)row["Correo"],
            };

            return newUser;
        }
    }
}
