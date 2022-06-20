using RealEstates.Models;
using RealEstates.Data;
using RealEstates.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RealEstates.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly ApplicationDbContext dbContext;

        public PropertiesService(ApplicationDbContext db)
        {
            this.dbContext = db;
        }

        public void Add(string district, int price, int floor, int maxFloor, int size, 
            int yardSize, int year, string propertyType, string buildingType)
        {
            var property = new Property
            {
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor > 255 ? null : (byte)floor,
                TotalFloors = maxFloor <= 0 || maxFloor > 255 ? null : (byte)maxFloor,
                Year = year < 1880 ? null : year,
                YardSize = yardSize <= 0 ? null : yardSize,
            };

            var dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }
            property.District = dbDistrict;

            var dbPropetryType = dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);
            if (dbPropetryType == null)
            {
                dbPropetryType = new PropertyType { Name = propertyType };
            }
            property.PropertyType = dbPropetryType;

            var dbBuildingType = dbContext.BuildingsType.FirstOrDefault(x => x.Name == buildingType);
            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }
            property.BuildingType = dbBuildingType;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();          
        }

        public decimal AveragePricePerSquareMeter()
        {
            return dbContext.Properties.Where(x => x.Price.HasValue)
               .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public decimal AveragePricePerSquareMeter(int districtId)
        {
            return dbContext.Properties.Where(x => x.Price.HasValue && x.DistrictId == districtId)
               .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public double AverageSize(int districtId)
        {
            return dbContext.Properties.Where(x => x.DistrictId == districtId)
               .Average(x => x.Size);
        }
        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties = dbContext.Properties
                .Where(x => x.Price >= minPrice 
                && x.Price <= maxPrice
                && x.Size >= minSize
                && x.Size <= maxPrice)
                .Select(x => new PropertyInfoDto
                {
                    Size = x.Size,
                    Price = x.Price ?? 0,
                    BuildingType = x.BuildingType.Name,
                    DistrictName = x.District.Name,
                    PropertyType = x.PropertyType.Name,
                })
                .ToList();

            return properties;
        }
    }
}
