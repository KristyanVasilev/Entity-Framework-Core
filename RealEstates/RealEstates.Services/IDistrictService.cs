using RealEstates.Services.DTOs;
using System;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IDistrictService
    {
        IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count);
    }
}
