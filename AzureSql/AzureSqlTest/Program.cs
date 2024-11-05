#pragma warning disable IDE0063

using System.Data.SqlClient;

namespace AzureSqlTest
{
    public class Program
    {
        static void Main()
        {
            // Set Inbound IP In Firewall Settings
            // SQL DB Server -> Settings -> Connection Strings
            string connectionString = "";

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
