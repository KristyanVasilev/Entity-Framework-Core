using System.Collections.Generic;

namespace RealEstates.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Properties = new HashSet<Property>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public int? Importance { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}