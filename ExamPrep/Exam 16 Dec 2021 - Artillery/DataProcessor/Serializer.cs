namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Linq;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context
                            .Shells
                            .Where(s => s.ShellWeight > shellWeight)
                            .ToArray()
                            .Select(s => new
                            {
                                ShellWeight = s.ShellWeight,
                                Caliber = s.Caliber,
                                Guns = s.Guns
                                .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                                .Select(g => new
                                {
                                    GunType = "AntiAircraftGun",
                                    GunWeight = g.GunWeight,
                                    BarrelLength = g.BarrelLength,
                                    Range = g.Range > 3000 ? "Long-range" : "Regular range"
                                })
                                .OrderByDescending(g => g.GunWeight)
                                .ToArray()
                            })
                            .OrderBy(s => s.ShellWeight)
                            .ToArray();


            var result = JsonConvert.SerializeObject(shells, Formatting.Indented);

            return result;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context.Guns
               .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
               .ToArray()
               .Select(g => new GunXmlExportModel
               {
                   Manufacturer = g.Manufacturer.ManufacturerName,
                   GunType = g.GunType.ToString(),
                   GunWeight = g.GunWeight,
                   BarrelLength = g.BarrelLength,
                   Range = g.Range,
                   Countries = g.CountriesGuns
                        .Where(c => c.Country.ArmySize > 4500000)
                        .Select(c => new GunCountryXmlExportModel
                        {
                            Country = c.Country.CountryName,
                            ArmySize = c.Country.ArmySize
                        })
                        .OrderBy(c => c.ArmySize)
                        .ToArray()
               })
               .OrderBy(g => g.BarrelLength)
               .ToArray();


            var result = XmlConverter.Serialize(guns, "Guns");

            return result;
        }
    }
}
