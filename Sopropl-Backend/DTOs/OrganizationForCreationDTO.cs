using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class OrganizationForCreationDTO
    {
        public string Id { get; set; }
        [Required]
        // [RegularExpression("^[a-zA-Z0-9& ]*$", ErrorMessage = "Enter valid orgnaization name not include special characters except space and & characters")]
        // [MaxLength(80)]
        // [MinLength(1)]
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }

    }
}