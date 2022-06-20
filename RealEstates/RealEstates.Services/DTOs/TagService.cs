using RealEstates.Data;
using RealEstates.Models;
using System;
using System.Linq;


namespace RealEstates.Services.DTOs
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPropertiesService propertiesService;

        public TagService(ApplicationDbContext dbContext, IPropertiesService propService)
        {
            this.dbContext = dbContext;
            this.propertiesService = propService;
        }

        public void Add(string name, int? importance = null)
        {
            var tag = new Tag
            {
                Name = name,
                Importance = importance
            };

            this.dbContext.Tags.Add(tag);
            this.dbContext.SaveChanges();
        }

        public void BulkTagProperties()
        {
            var allProperties = dbContext.Properties.ToList();

            foreach (var property in allProperties)
            {
                var avgPriceForDisrtict = this.propertiesService.AveragePricePerSquareMeter(property.DistrictId);

                if (property.Price > avgPriceForDisrtict)
                {
                    var tag = GetTag("Скъп-имот");
                    property.Tags.Add(tag);
                }

                if (property.Price < avgPriceForDisrtict)
                {
                    var tag = GetTag("Евтин-имот");
                    property.Tags.Add(tag);
                }

                var currentDate = DateTime.Now.AddYears(-15);
                if (property.Year.HasValue && property.Year.Value > currentDate.Year)
                {
                    var tag = GetTag("Нов-имот");
                    property.Tags.Add(tag);
                }

                if (property.Year.HasValue && property.Year.Value <= currentDate.Year)
                {
                    Tag tag = GetTag("Стар-имот");
                    property.Tags.Add(tag);
                }

                var avgSize = this.propertiesService.AverageSize(property.DistrictId);
                if (property.Size > avgSize)
                {
                    Tag tag = GetTag("Голям-имот");
                    property.Tags.Add(tag);
                }
                else
                {
                    Tag tag = GetTag("Малък-имот");
                    property.Tags.Add(tag);
                }

                if (property.Floor.HasValue && property.Floor.Value == 1)
                {
                    Tag tag = GetTag("Партер");
                    property.Tags.Add(tag);
                }

                if (property.Floor.HasValue && property.Floor.Value > 5)
                {
                    Tag tag = GetTag("Хубава гледка");
                    property.Tags.Add(tag);
                }
            }

            dbContext.SaveChanges();
        }

        private Tag GetTag(string name)
            => dbContext.Tags.FirstOrDefault(x => x.Name == name);       
    }
}
