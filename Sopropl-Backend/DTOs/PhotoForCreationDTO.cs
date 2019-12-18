using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Sopropl_Backend.DTOs
{
    public class PhotoForCreationDTO
    {
        // mybe projectId or organizationId
        public string OwnerId { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}