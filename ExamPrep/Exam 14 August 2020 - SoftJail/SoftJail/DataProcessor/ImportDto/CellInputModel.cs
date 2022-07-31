﻿namespace SoftJail.DataProcessor.ImportDto
{

    using System.ComponentModel.DataAnnotations;

    public class CellInputModel
    {
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        public bool HasWindow { get; set; }
    }
}