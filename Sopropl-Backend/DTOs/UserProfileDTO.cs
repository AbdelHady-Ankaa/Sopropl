using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class UserProfileDTO
    {
        public string Name { get; set; }
        [DataType(DataType.Text)]
        [MinLength(50, ErrorMessage = "Bio must be at least 100 characters")]
        [MaxLength(1500, ErrorMessage= "Bio cannot be more than 1500 characters")]
        public string Bio { get; set; }

        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{5}$|^\d{5}-\d{4}$", ErrorMessage="The postal code should be in the format 00000 or 00000-0000")]
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
    }
}