
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var SoftUniDb = new SoftUniContext();

            //Console.WriteLine(GetEmployeesFullInformation(SoftUniDb)); -Task 1
            //Console.WriteLine(GetEmployeesFullInformation(SoftUniDb)); -Task 2
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(SoftUniDb)); -Task 3
            Console.WriteLine(AddNewAddressToEmployee(SoftUniDb)); //Task 4
  
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var adress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(adress);
            context.SaveChanges();

            var employee = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            employee.Address = adress;
            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Select(x => new
                {
                    x.Address.AddressText
                })
                .Take(10)
                .ToList();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Salary,
                    x.Department.Name
                })
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} with {employee.Salary:f2}$");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees
                .Select(x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.MiddleName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.EmployeeId)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new { FirstName = x.FirstName, Salary = x.Salary })
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName);

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} - {item.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
