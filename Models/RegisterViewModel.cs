using System;
using System.ComponentModel.DataAnnotations;

namespace csharp_belt.Models
{
    public class RegisterViewModel: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage="Must be alphabetical.")]
        [Display(Name = "First Name")]      
        public string FirstName {get; set;}

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage="Must be alphabetical.")]
        [Display(Name = "Last Name")]                            
        public string LastName {get; set;}

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username {get; set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]        
        public string Confirm {get; set;}
    }
}