namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto;
    using System.Linq;
    using Newtonsoft.Json;
    using Artillery.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {

            var sb = new StringBuilder();

            var countries =
                XmlConverter.Deserializer<CountryXmlInputModel>(xmlString, "Countries");
            var validCountries = new HashSet<Country>();

            foreach (var currCountry in countries)
            {
                if (!IsValid(currCountry))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var country = new Country
                {
                    CountryName = currCountry.CountryName,
                    ArmySize = currCountry.ArmySize
                };

                sb.AppendLine($"Successfully import {country.CountryName} with {country.ArmySize} army personnel.");
                validCountries.Add(country);
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var manufactures =
                XmlConverter.Deserializer<ManufacturerXmlInputModel>(xmlString, "Manufacturers");
            var validManufactures = new HashSet<Manufacturer>();
            var validManufacturesNames = new HashSet<string>();

            foreach (var currManu in manufactures)
            {
                if (!IsValid(currManu))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (validManufacturesNames.Contains(currManu.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validManufacturesNames.Add(currManu.ManufacturerName);

                var manufacture = new Manufacturer
                {
                    ManufacturerName = currManu.ManufacturerName,
                    Founded = currManu.Founded
                };

                var foundInfo = manufacture.Founded.Split(", ").ToList();
                var town = foundInfo[foundInfo.Count - 2];
                var country = foundInfo.Last();

                sb.AppendLine($"Successfully import manufacturer {manufacture.ManufacturerName} founded in {town}, {country}.");
                validManufactures.Add(manufacture);
            }

            context.Manufacturers.AddRange(validManufactures);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var shells =
                XmlConverter.Deserializer<ShellXmlInputModel>(xmlString, "Shells");
            var validShells = new HashSet<Shell>();

            foreach (var currShell in shells)
            {
                if (!IsValid(currShell))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var shell = new Shell
                {
                    ShellWeight = currShell.ShellWeight,
                    Caliber = currShell.Caliber
                };

                sb.AppendLine($"Successfully import shell caliber #{shell.Caliber} weight {shell.ShellWeight} kg.");
                validShells.Add(shell);
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var validGuns = new HashSet<Gun>();

            var guns =
                JsonConvert.DeserializeObject<IEnumerable<GunJsonInputModel>>(jsonString);

            foreach (var currGun in guns)
            {
                if (!IsValid(currGun))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isEnumParsed = Enum.TryParse(currGun.GunType, true, out GunType gunType);
                if (!isEnumParsed)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var gun = new Gun
                {
                    ManufacturerId = currGun.ManufacturerId,
                    GunWeight = currGun.GunWeight,
                    BarrelLength = currGun.BarrelLength,
                    NumberBuild = currGun.NumberBuild,
                    Range = currGun.Range,
                    GunType = gunType,
                    ShellId = currGun.ShellId,
                };

                var countriesGuns = new HashSet<CountryGun>();

                foreach (var countyId in currGun.Countries)
                {
                    var country = context.Countries.Find(countyId.Id);

                    var countryGun = new CountryGun
                    {
                        Country = country,
                        Gun = gun
                    };

                    countriesGuns.Add(countryGun);
                }

                gun.CountriesGuns = countriesGuns;

                sb.AppendLine($"Successfully import gun {gun.GunType.ToString()} with a total weight of {gun.GunWeight} kg. and barrel length of {gun.BarrelLength} m.");
                validGuns.Add(gun);
            }

            context.Guns.AddRange(validGuns);
            context.SaveChanges();

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
