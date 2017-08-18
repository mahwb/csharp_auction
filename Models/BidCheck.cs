using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace csharp_belt.Models
{
    public class BidCheck: BaseEntity
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Starting bid is too low.")]
        [RegularExpression(@"^\d{0,8}(\.\d{1,4})?$", ErrorMessage = "Not a valid bid format.")]
        public double Amount {get; set;}
    }
}