using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopropl_Backend.DTOs
{
    public class ActivityForCreationDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Duration { get; set; }
        public ICollection<ArrowForCreationDTO> OutArrows { get; set; }

    }
}