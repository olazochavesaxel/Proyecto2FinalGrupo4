using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DTO;

namespace DataAccess.CRUDs
{
    public class AsesorCrudFactory : CrudFactory
    {
        public AsesorCrudFactory()
        {
            _sqlDAO = SqlDAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            var asesor = dto as Asesor;

            // Crear instrucción de ejecución para crear usuario
            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_ASESOR_PR" };

            // Agregar parámetros al procedimiento almacenado para tblUsuario

            sqlOperation.AddStringParameter("P_CEDULA", asesor.Cedula);
            sqlOperation.AddStringParameter("P_NOMBRE", asesor.Nombre);
            sqlOperation.AddStringParameter("P_PRIMER_APELLIDO", asesor.PrimerApellido);
            sqlOperation.AddStringParameter("P_SEGUNDO_APELLIDO", asesor.SegundoApellido);
            sqlOperation.AddStringParameter("P_DIRECCION", asesor.Direccion);
            sqlOperation.AddStringParameter("P_FOTO_PERFIL", asesor.FotoPerfil);
            sqlOperation.AddStringParameter("P_CONTRASENNA", asesor.Contrasenna);
            sqlOperation.AddStringParameter("P_TELEFONO", asesor.Telefono);
            sqlOperation.AddStringParameter("P_ESTADO", asesor.Estado);
            sqlOperation.AddStringParameter("P_ROL", "Asesor");
            sqlOperation.AddDateTimeParameter("P_FECHA_NACIMIENTO", asesor.FechaNacimiento);
            sqlOperation.AddDateTimeParameter("P_FECHA_EXPIRACION_OTP", asesor.FechaExpiracionOTP);
            sqlOperation.AddStringParameter("P_CORREO", asesor.Correo);
            sqlOperation.AddDoubleParameter("P_INGRESO_COMISIONES", asesor.IngresoComisiones);

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Update(BaseDTO dto)
        {
            var asesor = dto as Asesor;

            // Crear instrucción de ejecución para actualizar usuario
            var sqlOperation = new SqlOperation() { ProcedureName = "UDP_ASESOR_PR" };

            // Agregar parámetros al procedimiento almacenado para tblUsuario y tblCliente
            sqlOperation.AddIntParameter("P_ID", asesor.Id);
            sqlOperation.AddStringParameter("P_CEDULA", asesor.Cedula);
            sqlOperation.AddStringParameter("P_NOMBRE", asesor.Nombre);
            sqlOperation.AddStringParameter("P_PRIMER_APELLIDO", asesor.PrimerApellido);
            sqlOperation.AddStringParameter("P_SEGUNDO_APELLIDO", asesor.SegundoApellido);
            sqlOperation.AddStringParameter("P_DIRECCION", asesor.Direccion);
            sqlOperation.AddStringParameter("P_FOTO_PERFIL", asesor.FotoPerfil);
            sqlOperation.AddStringParameter("P_CONTRASENNA", asesor.Contrasenna);
            sqlOperation.AddStringParameter("P_TELEFONO", asesor.Telefono);
            sqlOperation.AddStringParameter("P_ESTADO", asesor.Estado);
            sqlOperation.AddStringParameter("P_ROL", "Asesor");
            sqlOperation.AddDateTimeParameter("P_FECHA_NACIMIENTO", asesor.FechaNacimiento);
            sqlOperation.AddDateTimeParameter("P_FECHA_EXPIRACION_OTP", asesor.FechaExpiracionOTP);
            sqlOperation.AddStringParameter("P_CORREO", asesor.Correo);
            sqlOperation.AddDoubleParameter("P_INGRESO_COMISIONES", asesor.IngresoComisiones); // Balance financiero

            // Ejecutar procedimiento en el DAO
            _sqlDAO.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO dto)
        {
            // Convertir DTO en Asesor
            var user = dto as Asesor;

            // Crear instrucción de ejecución
            var sqlOperation = new SqlOperation() { ProcedureName = "DELETE_ASESOR_BY_ID" };

            // Agregar parámetros al procedimiento almacenado
            sqlOperation.AddIntParameter("P_ID", user.Id);

            try
            {
                // Ejecutar procedimiento en el DAO
                _sqlDAO.ExecuteProcedure(sqlOperation);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al eliminar el asesor: {ex.Message}");
                // Podrías lanzar una nueva excepción o registrar el error de alguna manera
                throw new Exception("Ocurrió un error al eliminar el asesor.", ex);
            }
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstUsers = new List<T>();
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ALL_ASESORES" };
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
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ASESOR_BY_ID" };
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
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ASESOR_BY_CEDULA" };
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
            var sqlOperation = new SqlOperation() { ProcedureName = "RETRIEVE_ASESOR_BY_CORREO" };
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
        private Asesor BuildUser(Dictionary<string, object> row)
        {
            var newUser = new Asesor()
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
                IngresoComisiones = (double)row["IngresoComisiones"]
            };

            return newUser;
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }
    }
}