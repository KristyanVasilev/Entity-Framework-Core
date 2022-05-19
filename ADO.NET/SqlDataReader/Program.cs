using Microsoft.Data.SqlClient;
using System;

namespace SqlDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-KQDB4SL\\SQLEXPRESS;DataBase=SoftUni;Trusted_Connection=True;Encrypt=false";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Employees";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                var sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Console.WriteLine($"{sqlDataReader["FirstName"]} {sqlDataReader["LastName"]}" +
                        $" have annual salary {sqlDataReader["Salary"]:f2}$");
                }

                sqlDataReader.Close();
            }
        }
    }
}
