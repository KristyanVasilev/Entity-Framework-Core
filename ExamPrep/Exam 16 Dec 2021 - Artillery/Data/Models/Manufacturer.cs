namespace Artillery.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }
        public int Id { get; set; }

        [Required]
        //[Index(IsUnique = true)] upper version ef
        public string ManufacturerName { get; set; }

        [Required]
        public string Founded { get; set; }

        public ICollection<Gun> Guns { get; set; }
    }
}
