

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace DataAccess.DAOs
{
    public class SqlDAO
    {
        /*objeto que se encarga con la comunicaion con base de datos 
         solo utiliza stro procedures
        clase implementa patron como 
        SINGLETON
        para asegurar existencia de una unica instancia del Sql DAO */



        /*Paso 1 crear instancia privada en misma clase*/

        private static SqlDAO _instance;//GUION BAJO ES ESTANDAR EN ATRIBUTOS PRIVADOS
        private string _connectionString;

        /*Paso 2 Redifinir constructor defualt en privado*/
        private SqlDAO()
        {
           
            _connectionString = @"Data Source=DESKTOP-S248HQ3;Initial Catalog=Proyecto_2;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        }

        /*Paso 3 Definir metodo que expone la unica instancia de SqlDAO*/

        public static SqlDAO GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SqlDAO();
            }
            return _instance;

        }

        /*metodo que permite ejecutar store procedure en BD no devuleve nada solo en caso de excpetions*/
        public void ExecuteProcedure(SqlOperation sqlOperation)
        /*using se utiliza para que el objeto que se usa nace y muere en el momento que cumpla la finalidad del using*/
        {
            /*primer using usado para asosiarse a BD*/
            using (var conn = new SqlConnection(_connectionString))
            {
                /*segundo using asosiado a al comando que queremos ejectuar en esa BD*/
                using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    //Set de los parametros
                    foreach (var param in sqlOperation.Parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    conn.Open();
                    command.ExecuteNonQuery();
                }

            }


        }

        // Procedimiento stored procedure que retorna un set de datos.
        public List<Dictionary<string, object>> ExecuteQueryProcedure(SqlOperation sqlOperation)
        {
            var LstResults = new List<Dictionary<string, object>>();
            using (var conn = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            })
            {
                foreach (var param in sqlOperation.Parameters)
                {
                    command.Parameters.Add(param);
                }
                conn.Open();

                // Cambia la implementación respecto al procedure anterior.
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var RowDict = new Dictionary<string, object>();
                        for (var index = 0; index < reader.FieldCount; index++)
                        {
                            var key = reader.GetName(index);
                            var value = reader.GetValue(index);
                            // Se agregan valores al diccionario.
                            RowDict[key] = value;
                        }
                        LstResults.Add(RowDict);
                    }
                }
            }
            return LstResults;
        }
    }
}
