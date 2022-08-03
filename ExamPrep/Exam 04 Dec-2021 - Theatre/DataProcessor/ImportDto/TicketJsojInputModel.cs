namespace Theatre.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TicketJsojInputModel
    {
        [Range(1, 100)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
}
