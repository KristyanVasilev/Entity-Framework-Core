namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var plays = XmlConverter.Deserializer<PlaysXmlInputModel>(xmlString, "Plays");

            foreach (var currPlay in plays)
            {
                if (!IsValid(currPlay))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var IsValidDuration = TimeSpan.TryParseExact(
                   currPlay.Duration,
                   "c",
                   CultureInfo.InvariantCulture,
                   out TimeSpan duration);

                if (!IsValidDuration || duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isEnumParsed = Enum.TryParse(currPlay.Genre, true, out Genre genre);

                if (!isEnumParsed)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var play = new Play
                {
                    Title = currPlay.Title,
                    Duration = duration,
                    Rating = currPlay.Rating,
                    Description = currPlay.Description,
                    Screenwriter = currPlay.Screenwriter,
                    Genre = genre
                };

                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre.ToString()} and a rating of {play.Rating}!");
                context.Plays.Add(play);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var casts = XmlConverter.Deserializer<CastXmlInputModel>(xmlString, "Casts");

            foreach (var currCast in casts)
            {
                if (!IsValid(currCast))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var play = context.Plays.Find(currCast.PlayId);

                var cast = new Cast
                {
                    FullName = currCast.FullName,
                    IsMainCharacter = currCast.IsMainCharacter,
                    PhoneNumber = currCast.PhoneNumber,
                    Play = play
                };

                var mainOrLessr = cast.IsMainCharacter ? "main" : "lesser";

               sb.AppendLine($"Successfully imported actor {cast.FullName} as a {mainOrLessr} character!");
               context.Casts.Add(cast);
               context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var theatres =
                JsonConvert.DeserializeObject<IEnumerable<TheatreJsonInputModel>>(jsonString);

            foreach (var currTheatre in theatres)
            {
                if (!IsValid(currTheatre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var tickets = new HashSet<Ticket>();
                foreach (var cuurTicket in currTheatre.Tickets)
                {
                    if (!IsValid(cuurTicket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var play = context.Plays.Find(cuurTicket.PlayId);

                    var ticket = new Ticket
                    {
                        Price = cuurTicket.Price,
                        RowNumber = cuurTicket.RowNumber,
                        Play = play
                    };

                    tickets.Add(ticket);
                }


                var thatre = new Theatre
                {
                    Name = currTheatre.Name,
                    Director = currTheatre.Director,
                    NumberOfHalls = currTheatre.NumberOfHalls,
                    Tickets = tickets
                };

                sb.AppendLine($"Successfully imported theatre {thatre.Name} with #{thatre.Tickets.Count} tickets!");
                context.Theatres.Add(thatre);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
