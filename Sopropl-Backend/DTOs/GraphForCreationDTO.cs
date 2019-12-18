using System;
using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class GraphForCreationDTO
    {
        [Required]
        public ActivityForCreationDTO StartNode { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? EarlyStart { get; set; }
    }
}