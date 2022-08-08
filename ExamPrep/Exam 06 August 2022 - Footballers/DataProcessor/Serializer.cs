namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context
            .Coaches
            .ToArray()
            .Where(c => c.Footballers.Any())
            .Select(c => new CoachXmlExportModel()
            {
                FootballersCount = c.Footballers.Count.ToString(),
                CoachName = c.Name,
                Footballers = c.Footballers.Select(f => new FootballerXmlExportModel
                {
                    Name = f.Name,
                    Position = f.PositionType.ToString()
                })
                .OrderBy(f => f.Name)
                .ToArray()
            })
            .OrderByDescending(c => c.FootballersCount)
            .ThenBy(c => c.CoachName)
            .ToArray();


            var result = XmlConverter.Serialize(coaches, "Coaches");

            return result;
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context
                  .Teams
                  .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                  .ToArray()
                  .Select(t => new
                  {
                      Name = t.Name,
                      Footballers = t.TeamsFootballers
                     .Where(f => f.Footballer.ContractStartDate >= date)
                     .OrderByDescending(f => f.Footballer.ContractEndDate)
                     .ThenBy(f => f.Footballer.Name)
                     .Select(f => new
                     {
                         FootballerName = f.Footballer.Name,
                         ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                         ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                         BestSkillType = f.Footballer.BestSkillType.ToString(),
                         PositionType = f.Footballer.PositionType.ToString(),
                     })
                    .ToArray()
                  })
                  .OrderByDescending(t => t.Footballers.Length)
                  .ThenBy(t => t.Name)
                  .Take(5)
                  .ToArray();

            string result = JsonConvert.SerializeObject(teams, Formatting.Indented);

            return result;
        }
    }
}
