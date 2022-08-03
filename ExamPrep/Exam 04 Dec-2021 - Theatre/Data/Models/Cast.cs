﻿namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Cast
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public bool IsMainCharacter { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int PlayId { get; set; }

        public Play Play { get; set; }
    }
}

