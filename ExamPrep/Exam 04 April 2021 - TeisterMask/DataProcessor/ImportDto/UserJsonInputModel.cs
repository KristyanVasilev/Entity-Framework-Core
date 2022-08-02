namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class UserJsonInputModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression("^[A-Za-z0-9]+$")]

        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}")]
        public string Phone { get; set; }

        public IEnumerable<int> Tasks { get; set; }
    }
}