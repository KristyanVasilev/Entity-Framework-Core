using System;
using System.Collections.Generic;

#nullable disable

namespace ORM_Fundamentals.Models
{
    public partial class DepartmentIdAndAvgSalary
    {
        public int DepartmentId { get; set; }
        public decimal? AvarageSalary { get; set; }
    }
}
