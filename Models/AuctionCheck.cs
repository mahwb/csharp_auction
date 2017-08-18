using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomDataAnnotations;

namespace csharp_belt.Models
{
    public class AuctionCheck: BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string Name {get; set;}
        [Required]
        [MinLength(2)]
        public string Description {get; set;}
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting bid is too low.")]
        [RegularExpression(@"^\d{0,8}(\.\d{0,2})?$", ErrorMessage = "Not a valid bid format.")]
        [Display(Name = "Starting Bid")]
        public double Bid {get; set;}
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        [CurrentDate(ErrorMessage = "Date needs to be current or future date.")]
        public DateTime EndDate {get; set;}
    }
}