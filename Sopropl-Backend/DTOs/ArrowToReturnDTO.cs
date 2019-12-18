using System;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.DTOs
{
    public class ArrowToReturnDTO
    {
        public ActivityToReturnDTO ToActivity { get; set; }
        public double ConstraintValue { get; set; }
        public ArrowType Type { get; set; }
    }
}