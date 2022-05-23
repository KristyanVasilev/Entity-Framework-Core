using Microsoft.Data.SqlClient;
using System;

namespace _01.InitialSetup
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateDataBase();

            string connectionString = "Server=DESKTOP-KQDB4SL\\SQLEXPRESS;DataBase=MinionsDB;Trusted_Connection=True;Encrypt=false";

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            string[] DataBaseStatements = CreateDataBaseStatements();

            foreach (var query in DataBaseStatements)
            {
                ExecuteNonQuery(connection, query);
            }

        }
        private static string[] CreateDataBaseStatements()
        {
            string[] statements = new string[]
            {
                "CREATE TABLE Countries(Id INT PRIMARY KEY,[Name] VARCHAR(50))",
                "CREATE TABLE Towns(Id INT PRIMARY KEY,[Name] VARCHAR(50),CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                "CREATE TABLE Minions(Id INT PRIMARY KEY,[Name] VARCHAR(50),Age INT,TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY,[Name] VARCHAR(50))",
                "CREATE TABLE Villains(Id INT PRIMARY KEY,[Name] VARCHAR(50),EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))","CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id)CONSTRAINT PK_MinionsVillians PRIMARY KEY(MinionId, VillainId))"
            };

            return statements;
        }

        private static void ExecuteNonQuery(SqlConnection connection, string query)
        {
            using SqlCommand sqlCommand = new SqlCommand(query, connection);
            sqlCommand.ExecuteNonQuery();
        }

        private static void CreateDataBase()
        {
            string connectionString = "Server=DESKTOP-KQDB4SL\\SQLEXPRESS;DataBase=master;Trusted_Connection=True;Encrypt=false";
            var connection = new SqlConnection(connectionString);

            connection.Open();
            var query = "CREATE DATABASE MinionsDB";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            sqlCommand.ExecuteNonQuery();

            connection.Close();
        }
    }
}
