namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var coaches =
                XmlConverter.Deserializer<CoachXmlInputModel>(xmlString, "Coaches");

            var validCoaches = new HashSet<Coach>();

            foreach (var currCoach in coaches)
            {
                if (!IsValid(currCoach))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var coach = new Coach
                {
                    Name = currCoach.Name,
                    Nationality = currCoach.Nationality
                };

                var validFootballers = new HashSet<Footballer>();

                foreach (var currFootballer in currCoach.Footballers)
                {
                    if (!IsValid(currFootballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var IsValidStartDate = DateTime.TryParseExact(
                        currFootballer.ContractStartDate,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime startDate);

                    if (!IsValidStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var IsValidEndDate = DateTime.TryParseExact(
                        currFootballer.ContractEndDate,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime endDate);

                    if (!IsValidStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (startDate > endDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var bestSkillType = (BestSkillType)currFootballer.BestSkillType;
                    var positionType = (PositionType)currFootballer.PositionType;

                    var footballer = new Footballer
                    {
                        Name = currFootballer.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        BestSkillType = bestSkillType,
                        PositionType = positionType
                    };

                    validFootballers.Add(footballer);
                }

                coach.Footballers = validFootballers;

                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
                validCoaches.Add(coach);
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var validTeams = new HashSet<Team>();

            var teams =
                JsonConvert.DeserializeObject<IEnumerable<TeamJsonInputModel>>(jsonString);

            foreach (var currTeam in teams)
            {
                if (!IsValid(currTeam))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                int throphies;
                var IsValidThrophies = int.TryParse(currTeam.Trophies, out throphies);

                if (!IsValidThrophies)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var team = new Team
                {
                    Name = currTeam.Name,
                    Nationality = currTeam.Nationality,
                    Trophies = throphies
                };

                var validTeamsFootballers = new HashSet<TeamFootballer>();

                foreach (var footballerID in currTeam.Footballers.Distinct())
                {
                    var footballer = context.Footballers.Find(footballerID);

                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var teamFootballer = new TeamFootballer
                    {
                        TeamId = team.Id,
                        FootballerId = footballer.Id
                    };

                    validTeamsFootballers.Add(teamFootballer);
                }

                team.TeamsFootballers = validTeamsFootballers;

                sb.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
                validTeams.Add(team);
            }

            context.Teams.AddRange(validTeams);
            context.SaveChanges();

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
