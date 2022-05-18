using Microsoft.Data.SqlClient;
using System;

namespace Introduction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-KQDB4SL\\SQLEXPRESS;DataBase=SoftUni;Trusted_Connection=True;Encrypt=false";
            var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var query = "UPDATE Employees SET Salary = Salary + 10";
                SqlCommand sqlCommand = new SqlCommand(query, connection);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
                Console.WriteLine(rowsAffected);
            }
        }
    }
}
