using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        // [Required]
        // [DataType(DataType.PhoneNumber)]
        // public string PhoneNumber { get; set; }
        // [Required]
        // public string Country { get; set; }
        // [Required]
        // public string City { get; set; }
    }
}