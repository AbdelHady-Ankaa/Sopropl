using System.ComponentModel.DataAnnotations;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.DTOs
{
    public class ArrowForCreationDTO2
    {
        [Required]
        public ActivityArrowForCreationDTO FromActivity { get; set; }
        [Required]
        public ActivityArrowForCreationDTO ToActivity { get; set; }
        [Required]
        public double? ConstraintValue { get; set; }
        [Required]
        public ArrowType? Type { get; set; }
    }
    public class ActivityArrowForCreationDTO
    {
        [Required]
        public string Name { get; set; }
    }

}