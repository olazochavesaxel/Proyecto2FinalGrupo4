using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

var connStr = @"Data Source=DESKTOP-GQ1EGAS\user;Initial Catalog=Proyecto_2;Integrated Security=True;Trust Server Certificate=True";

try
{
    using var conn = new SqlConnection(connStr);
    conn.Open();
    Console.WriteLine("✅ Conexión exitosa.");
}
catch (Exception ex)
{
    Console.WriteLine("❌ Error: " + ex.Message);
}
