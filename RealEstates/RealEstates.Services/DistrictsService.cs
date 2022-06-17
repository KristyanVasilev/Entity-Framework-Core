using RealEstates.Data;
using RealEstates.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class DistrictsService : IDistrictService
    {
        private readonly ApplicationDbContext dbContext;

        public DistrictsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = dbContext.Districts.Select(x => new DistrictInfoDto
            {
                Name = x.Name,
                PropertiesCount = x.Properties.Count(),
                AvgPricePerSqrMeter =
                    x.Properties.Where(p => p.Price.HasValue)
                     .Average(p => p.Price / (decimal)p.Size) ?? 0,
            })
            .OrderByDescending(x => x.AvgPricePerSqrMeter)
            .Take(count)
            .ToList();

            return districts;
        }
    }
}
