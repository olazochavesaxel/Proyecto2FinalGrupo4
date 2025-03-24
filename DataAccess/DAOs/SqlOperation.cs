using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace DataAccess.DAOs
{
    /*Clase con instrucciones de lo que tiene que hacer el SQL DAO*/
    public class SqlOperation
    {
        public string ProcedureName { get; set; }
        public List<SqlParameter> Parameters { get; set; }
        public SqlOperation()
        {
            Parameters = new List<SqlParameter>();
        }

        public void AddStringParameter(string parameterName, string value)
        {
            Parameters.Add(new SqlParameter(parameterName, value));
        }

        public void AddIntParameter(string parameterName, int value)
        {
            Parameters.Add(new SqlParameter(parameterName, value));
        }

        public void AddDoubleParameter(string parameterName, double value)
        {
            Parameters.Add(new SqlParameter(parameterName, value));
        }

        public void AddDateTimeParameter(string parameterName, DateTime value)
        {
            Parameters.Add(new SqlParameter(parameterName, value));
        }

        public void AddDecimalParameter(string paramName, decimal value)
        {
            SqlParameter parameter = new SqlParameter(paramName, SqlDbType.Decimal)
            {
                Precision = 10,
                Scale = 2,
                Value = value
            };
            Parameters.Add(parameter);
        }
    }
}
