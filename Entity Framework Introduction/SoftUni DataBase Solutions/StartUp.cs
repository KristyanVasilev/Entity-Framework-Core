
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var SoftUniDb = new SoftUniContext();

            //Console.WriteLine(GetEmployeesFullInformation(SoftUniDb)); //Task 3
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(SoftUniDb)); //Task 4
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(SoftUniDb));//Task 5
            //Console.WriteLine(AddNewAddressToEmployee(SoftUniDb)); //Task 6
            //Console.WriteLine(GetEmployeesInPeriod(SoftUniDb)); //Task 7
            //Console.WriteLine(GetAddressesByTown(SoftUniDb)); //Task 8
            //Console.WriteLine(GetEmployee147(SoftUniDb)); //Task 9
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(SoftUniDb)); //Task 10
            //Console.WriteLine(GetLatestProjects(SoftUniDb)); //Task 11
            //Console.WriteLine(IncreaseSalaries(SoftUniDb)); //Task 12
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(SoftUniDb)); //Task 13
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(SoftUniDb)); //Task 14
            Console.WriteLine(RemoveTown(SoftUniDb)); //Task 15

        }
        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .Include(x => x.Addresses)
                .FirstOrDefault(x => x.Name == "Seattle");
            var addressesIDs = town.Addresses.Select(x => x.AddressId).ToList();
            var employees = context.Employees.Where(x => x.AddressId.HasValue && addressesIDs.Contains(x.AddressId.Value)).ToList();

            foreach (var emp in employees)
            {
                emp.AddressId = null;
            }

            foreach (var addressID in addressesIDs)
            {
               var address = context.Addresses.FirstOrDefault(x => x.AddressId == addressID);

                context.Addresses.Remove(address);
            }

            context.Towns.Remove(town);
            context.SaveChanges();

            var result = $"{addressesIDs.Count()} addresses in Seattle were deleted";
            return result;
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Salary,
                    x.JobTitle
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Department.Name == "Engineering"
                || x.Department.Name == "Tool Design" 
                || x.Department.Name == "Marketing"
                || x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var emp in employees)
            {
                emp.Salary *= 1.12M;
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects               
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .ToList();

            foreach (var project in projects.OrderBy(x => x.Name))
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var department = context.Departments
                .Where(x => x.Employees.Count() > 5)
                .Select(x => new 
                {
                    Name = x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(e => new 
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList()
                })
                .ToList();

            foreach (var dep in department)
            {
                sb.AppendLine($"{dep.Name} - {dep.ManagerFirstName} {dep.ManagerLastName}");
                foreach (var emp in dep.Employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees
                .Select(x => new 
                {
                    EmployeeId = x.EmployeeId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    EmployeesProjects = x.EmployeesProjects.Select(p => new 
                    {
                        ProjectName = p.Project.Name
                    })
                    .OrderBy(p => p.ProjectName)
                    .ToList()
                })
                .FirstOrDefault(x => x.EmployeeId == 147);

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.EmployeesProjects)
            {
                sb.AppendLine($"{project.ProjectName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var addresses = context.Addresses
                .Include(x => x.Employees)
                .Include(x => x.Town)
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    Count = x.Employees.Count(),
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TownName)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.Count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFisrtName = x.FirstName,
                    EmployeeLastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projetcs = x.EmployeesProjects.Select(p => new
                    {
                        Name = p.Project.Name,
                        StartDate = p.Project.StartDate,
                        EndDate = p.Project.EndDate
                    })
                })
                .Take(10)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.EmployeeFisrtName} {employee.EmployeeLastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var emplProject in employee.Projetcs)
                {
                    var endDate = emplProject.EndDate.HasValue
                         ? emplProject.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                         : "not finished";

                    sb.AppendLine($"--{emplProject.Name} - {emplProject.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {emplProject.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
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
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:f2}");
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
