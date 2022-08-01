namespace VaporStore.DataProcessor
{
	using System;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var genres = context
                 .Genres
                 .ToArray()
                 .Where(g => genreNames.Contains(g.Name))
                 .Select(g => new
                 {
                     Id = g.Id,
                     Genre = g.Name,
                     Games = g.Games
                         .Where(ga => ga.Purchases.Any())
                         .Select(ga => new
                         {
                             Id = ga.Id,
                             Title = ga.Name,
                             Developer = ga.Developer.Name,
                             Tags = String.Join(", ", ga.GameTags
                                 .Select(gt => gt.Tag.Name)
                                 .ToArray()),
                             Players = ga.Purchases.Count
                         })
                         .OrderByDescending(ga => ga.Players)
                         .ThenBy(ga => ga.Id)
                         .ToArray(),
                     TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)
                 })
                 .OrderByDescending(g => g.TotalPlayers)
                 .ThenBy(g => g.Id)
                 .ToArray();

            string result = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return result;
        }

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{

          

            return "TODO";
        }
	}
}