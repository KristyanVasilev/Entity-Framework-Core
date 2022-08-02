namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            return "TODO";
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {

            var employees = context
                 .Employees
                 .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                 .ToArray()
                 .Select(e => new
                 {
                     e.Username,
                     Tasks = e.EmployeesTasks
                         .Where(et => et.Task.OpenDate >= date)
                         .ToArray()
                         .OrderByDescending(et => et.Task.DueDate)
                         .ThenBy(et => et.Task.Name)
                         .Select(et => new
                         {
                             TaskName = et.Task.Name,
                             OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                             DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                             LabelType = et.Task.LabelType.ToString(),
                             ExecutionType = et.Task.ExecutionType.ToString()
                         })
                         .ToArray()
                 })
                 .OrderByDescending(e => e.Tasks.Length)
                 .ThenBy(e => e.Username)
                 .Take(10)
                 .ToArray();


            string result = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return result;
        }
    }
}

