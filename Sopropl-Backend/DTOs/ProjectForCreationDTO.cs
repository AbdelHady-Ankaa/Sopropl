using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class ProjectForCreationDTO
    {
        [Required]
        // [RegularExpression("(/^[a-z\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}$/i)")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ProjectType { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }

    }
}