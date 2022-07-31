namespace SoftJail.DataProcessor.ImportDto
{

    using System.ComponentModel.DataAnnotations;

    public class MailInputModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"^([A-z0-9\s]+ str.)$")]
        public string Address { get; set; }
    }
}

