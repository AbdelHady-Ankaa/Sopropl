using System.ComponentModel.DataAnnotations;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.DTOs
{
    public class ArrowForCreationDTO
    {
        [Required]
        public ActivityForCreationDTO ToActivity { get; set; }
        [Required]
        public double ConstraintValue { get; set; }
        [Required]
        public ArrowType Type { get; set; }
    }
}