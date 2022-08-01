namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
            var sb = new StringBuilder();

            var games =
                JsonConvert.DeserializeObject<IEnumerable<GameJsonInputModel>>(jsonString);

            foreach (var currGame in games)
            {
                if (!IsValid(currGame) || !currGame.Tags.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                
                var developer = context.Developers.FirstOrDefault(x => x.Name == currGame.Developer)
                    ?? new Developer { Name = currGame.Developer };

                var genre = context.Genres.FirstOrDefault(x => x.Name == currGame.Genre)
                    ?? new Genre { Name = currGame.Genre};

                var game = new Game
                {
                   Name = currGame.Name,
                   ReleaseDate = currGame.ReleaseDate.Value,
                   Price = currGame.Price,
                   Developer = developer,
                   Genre = genre
                };

                foreach (var currTag in currGame.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(x => x.Name == currTag)
                    ?? new Tag { Name = currTag };

                    game.GameTags.Add(new GameTag { Tag = tag });
                }

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
                context.Games.Add(game);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var sb = new StringBuilder();

            var users =
                JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(jsonString);

            foreach (var currUser in users)
            {
                if (!IsValid(currUser) || !currUser.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    FullName = currUser.FullName,
                    Username = currUser.Username,
                    Email = currUser.Email,
                    Age = currUser.Age,
                    Cards = currUser.Cards.Select(x => new Card
                    {
                        Number = x.Number,
                        Cvc = x.Cvc,
                        Type = x.Type.Value
                    })
                    .ToList()
                };

                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                context.Users.Add(user);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var sb = new StringBuilder();

            var purchases = XmlConverter.Deserializer<PurchaseXmlInputModel>(xmlString, "Purchases");

            foreach (var currPuchase in purchases)
            {
                if (!IsValid(currPuchase))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var IsValidDate = DateTime.TryParseExact(
                   currPuchase.Date,
                   "dd/MM/yyyy HH:mm",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out DateTime date);

                if (!IsValidDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var card = context.Cards.FirstOrDefault(x => x.Number == currPuchase.Card);
                var game = context.Games.FirstOrDefault(x => x.Name == currPuchase.Title);

                var purchase = new Purchase
                {
                    Game = game,
                    Type = currPuchase.Type.Value,
                    ProductKey = currPuchase.Key,
                    Card = card,
                    Date = date
                };

                var user = context.Users.FirstOrDefault(x => x.Id == card.UserId);

                sb.AppendLine($"Imported {purchase.Game.Name} for {user.Username}");
                context.Purchases.Add(purchase);
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