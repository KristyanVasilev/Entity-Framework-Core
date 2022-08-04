﻿namespace Artillery.Data.Models
{
    public class CountryGun
    {
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public int GunId { get; set; }

        public Gun Gun { get; set; }
    }
}
//•	CountryId – Primary Key integer, foreign key (required)
//•	GunId – Primary Key integer, foreign key (required)
