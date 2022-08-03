namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context
                 .Theatres
                 .Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count > 20)
                 .ToArray()
                 .Select(x => new
                 {
                     Name = x.Name,
                     Halls = x.NumberOfHalls,
                     TotalIncome = x.Tickets
                                    .Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                                    .Sum(t => t.Price),
                     Tickets = x.Tickets
                         .Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                         .Select(t => new
                         {
                             Price = t.Price,
                             RowNumber = t.RowNumber
                         })
                         .OrderByDescending(t => t.Price)
                         .ToArray()
                 })
                 .OrderByDescending(x => x.Halls)
                 .ThenBy(x => x.Name)
                 .ToArray();


            string result = JsonConvert.SerializeObject(theatres, Formatting.Indented);

            return result;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays
               .Where(p => p.Rating <= rating)
               .ToArray()
               .Select(p => new PlayXmlExportModel
               {
                   Title = p.Title,
                   Duration = p.Duration.ToString("c"),
                   Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                   Genre = p.Genre.ToString(),
                   Actors = context.Casts
                            .Where(c => c.IsMainCharacter && c.Play.Id == p.Id)
                            .Select(s => new ActorXmlExportModel
                            {
                                FullName = s.FullName,
                                MainCharacter = $"Plays main character in '{p.Title}'."
                            })
                            .OrderByDescending(a => a.FullName)
                            .ToArray()
               })
               .OrderBy(p => p.Title)
               .ThenByDescending(p => p.Genre)
               .ToArray();

            var result = XmlConverter.Serialize(plays, "Plays");

            return result;
        }
    }
}