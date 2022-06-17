using RealEstates.Services.DTOs;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Add(string district, int price,
        int floor, int maxFloor, int size, int yardSize,
        int year, string propertyType, string buildingType);

        decimal AveragePricePerSquareMeter();

        IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize);
    }
}
