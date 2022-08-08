namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    public class TeamJsonInputModel
    {
        [Required]
        [RegularExpression(@"^[A-Za-z0-9 \.-]+$")]
        [StringLength(40,MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; }

        [Required]
        public string Trophies { get; set; }

        public int[] Footballers { get; set; }
    }
}