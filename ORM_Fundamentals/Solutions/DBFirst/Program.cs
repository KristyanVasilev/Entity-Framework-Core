using ORM_Fundamentals.Models;
using System;
using System.Linq;

namespace ORM_Fundamentals
{
    //Microsoft.EntityFrameworkCore.SqlServer
    //Microsoft.EntityFrameworkCore.Design
    
    //To connect with DB in Developer PowerShell write - dotnet ef dbcontext scaffold "Server={Your server name or .};Integrated Security=true;Database={Name}" Microsoft.EntityFrameworkCore.SqlServer
    
    //dotnet tool install --global dotnet-ef / Installing tool, if have a problem!!!
    
    //If Your startup project doesn't reference Microsoft.EntityFrameworkCore.Design, add to your PropertyGroup section following entry: <GenerateRuntimeConfigurationFiles>True</GenerateRuntimeConfigurationFiles>

    public class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();
            Console.WriteLine($"Employees count is {db.Employees.Count()}");
            Console.WriteLine();

            var employees = db.Employees
                .Where(x => x.FirstName.StartsWith("N"))
                .OrderByDescending(x => x.Salary)
                .Select(x => new { x.FirstName, x.LastName, x.Salary })
                .ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} with salary = {employee.Salary:f2}$");
            }

            Console.WriteLine();
            var departments = db.Employees
                .GroupBy(x => x.Department.Name)
                .Select(x => new { Name = x.Key, Count = x.Count() })
                .ToList();

            foreach (var department in departments)
            {
                Console.WriteLine($"{department.Name} => {department.Count}");
            }

            //Insert town into Towns
            //db.Towns.Add(new Town { Name = "Dupnitsa" });
            //db.SaveChanges();
        }
    }
}
