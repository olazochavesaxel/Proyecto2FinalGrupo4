using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        var connStr = @"Data Source=DESKTOP-GQ1EGAS\user;Initial Catalog=Proyecto_2;Integrated Security=True;TrustServerCertificate=True";

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
    }
}
