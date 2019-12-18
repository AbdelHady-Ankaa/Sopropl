using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class UserForLoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}