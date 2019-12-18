using System;
using System.Collections.Generic;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.DTOs
{
    public class ActivityToReturnDTO
    {
        public string Name { get; set; }
        public ActivityType Type { get; set; }
        public ActivityState State { get; set; }
        public string EarlyStart { get; set; }
        public DateTime? EarlyFinish { get; set; }
        public DateTime? LateStart { get; set; }
        public DateTime? LateFinish { get; set; }
        public double FreeFloat { get; set; }
        public double TotalFloat { get; set; }
        public double Duration { get; set; }
        public byte Priority { get; set; }
        public double EstimatedHours { get; set; }
        public double HoursSpent { get; set; }
        public string Description { get; set; }
        public ICollection<ArrowToReturnDTO> OutArrows { get; set; }
    }
}