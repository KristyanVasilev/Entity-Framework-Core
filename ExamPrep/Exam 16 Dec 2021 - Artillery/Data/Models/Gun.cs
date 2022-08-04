﻿namespace Artillery.Data.Models
{
    using Artillery.Data.Models.Enums;
    using System.Collections.Generic;

    public class Gun
    {
        public Gun()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }
        public int Id { get; set; }

        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }

        public int GunWeight { get; set; }

        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        public int Range { get; set; }

        public GunType GunType { get; set; }

        public int ShellId { get; set; }

        public Shell Shell { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
