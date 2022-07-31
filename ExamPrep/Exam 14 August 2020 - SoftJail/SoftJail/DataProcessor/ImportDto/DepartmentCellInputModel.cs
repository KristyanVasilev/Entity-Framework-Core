namespace SoftJail.DataProcessor.ImportDto
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class DepartmentCellInputModel
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        public List<CellInputModel> Cells { get; set; }
    }
}