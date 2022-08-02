namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var projects = XmlConverter.Deserializer<ProjectXmlInputModel>(xmlString, "Projects");

            foreach (var currProject in projects)
            {
                if (!IsValid(currProject))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var IsValidOpenDate = DateTime.TryParseExact(
                   currProject.OpenDate,
                   "dd/MM/yyyy",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out DateTime openDate);

                if (!IsValidOpenDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? dueDate = null;
                var IsValidDueDate = DateTime.TryParseExact(
                  currProject.DueDate,
                  "dd/MM/yyyy",
                  CultureInfo.InvariantCulture,
                  DateTimeStyles.None,
                  out DateTime dueDateValue);

                if (IsValidDueDate)
                {
                    dueDate = dueDateValue;
                }
               

                var project = new Project
                {
                    Name = currProject.Name,
                    OpenDate = openDate,
                    DueDate = dueDate,
                    Tasks = new List<Task>()
                };

                foreach (var task in currProject.Tasks)
                {
                    if (!IsValid(task))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var IsValidTaskOpenDate = DateTime.TryParseExact(
                       task.TaskOpenDate,
                       "dd/MM/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime taskOpenDate);

                    if (!IsValidTaskOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var IsValidTaskDueDate = DateTime.TryParseExact(
                       task.TaskDueDate,
                       "dd/MM/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime taskDueDate);

                    if (!IsValidTaskDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < project.OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (project.DueDate.HasValue && taskDueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validTask = new Task
                    {
                        Name = task.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)task.ExecutionType,
                        LabelType = (LabelType)task.LabelType,
                        Project = project
                    };

                    project.Tasks.Add(validTask);
                }

                sb.AppendLine($"Successfully imported project - {project.Name} with {project.Tasks.Count} tasks.");
                context.Projects.Add(project);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var users =
                JsonConvert.DeserializeObject<IEnumerable<UserJsonInputModel>>(jsonString);

            foreach (var currUser in users)
            {
                if (!IsValid(currUser))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var user = new Employee
                {
                    Username = currUser.Username,
                    Email = currUser.Email,
                    Phone = currUser.Phone,
                };

                var employeeTasks = new HashSet<EmployeeTask>();

                foreach (var taskId in currUser.Tasks.Distinct())
                {
                    var task = context.Tasks.Find(taskId);
                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var currEmployeeTask = new EmployeeTask
                    {
                        Employee = user,
                        Task = task,
                    };

                    employeeTasks.Add(currEmployeeTask);
                }

                user.EmployeesTasks = employeeTasks;

                sb.AppendLine($"Successfully imported employee - {user.Username} with {user.EmployeesTasks.Count} tasks.");
                context.Employees.Add(user);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}