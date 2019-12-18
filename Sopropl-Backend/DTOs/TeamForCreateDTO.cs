using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class TeamForCreateDTO
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}