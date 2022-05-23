using Microsoft.Data.SqlClient;
using System;

namespace SqlInjection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter username:");
            var username = Console.ReadLine();
            Console.WriteLine("Please enter password:");
            var password = Console.ReadLine();


            string connectionString = "Server=DESKTOP-KQDB4SL\\SQLEXPRESS;DataBase=Minions;Trusted_Connection=True;Encrypt=false";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sqlInjection = $"SELECT COUNT(*) FROM Users WHERE Name = '{username}' AND Password = '{password}'";
                //Uses data directly from input
                //if username = ' OR 1=1 -- will always pass

                var rightQuery = $"SELECT COUNT(*) FROM Users WHERE Name = @Username AND Password = @Password";
                SqlCommand sqlCommand = new SqlCommand(sqlInjection, connection);
                sqlCommand.Parameters.AddWithValue("@Username", username);
                sqlCommand.Parameters.AddWithValue("@Password", password);                

                int usersCount = (int)sqlCommand.ExecuteScalar();

                if (usersCount > 0)
                {
                    Console.WriteLine("Access granted! Welcome!");
                }
                else
                {
                    Console.WriteLine("Access denied!");
                }
            }
        }
    }
}
