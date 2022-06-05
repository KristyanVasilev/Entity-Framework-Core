namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            var result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();

            var producer = context.Producers.FirstOrDefault(x => x.Id == producerId);

            var albums = producer.Albums.Select(x => new
            {
                x.Name,
                x.ReleaseDate,
                x.Price,
                x.Songs
            }).OrderByDescending(x => x.Price).ToList();



            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}")
                  .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                  .AppendLine($"-ProducerName: {producer.Name}")
                  .AppendLine("-Songs:");

                var songs = album.Songs
                    .Select(x => new
                    {
                        x.Name,
                        x.Writer,
                        x.Price
                    })
                  .OrderByDescending(x => x.Name)
                  .ThenBy(x => x.Writer.Name);

                var count = 1;

                foreach (var song in songs)
                {
                    sb.AppendLine($"---#{count++}")
                      .AppendLine($"---SongName: {song.Name}")
                      .AppendLine($"---Price: {song.Price:f2}")
                      .AppendLine($"---Writer: {song.Writer.Name}");
                }

                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sb = new StringBuilder();

            var songs = context.Songs
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new
                {
                    SongName = x.Name,
                    PerformerFullName = x.SongPerformers
                                         .Select(x => x.Performer.FirstName + " " + x.Performer.LastName)
                                         .FirstOrDefault(),
                    WirterName = x.Writer.Name,
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.WirterName)
                .ThenBy(x => x.PerformerFullName);

            var count = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{count++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WirterName}")
                    .AppendLine($"---Performer: {song.PerformerFullName}")
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration:c}");
            }
            return sb.ToString().TrimEnd();
        }
        //Name, Performer Full Name, Writer Name, Album Producer and Duration(in format("c")). Sort the Songs by their Name(ascending), by Writer(ascending) and by Performer(ascending
    }
}
