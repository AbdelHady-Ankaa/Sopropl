using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sopropl_Backend.Models
{
    public partial class Activity : ICloneable
    {
        public Activity()
        {
            this.Resources = new List<Resource>();
            this.InArrows = new List<Arrow>();
            this.OutArrows = new List<Arrow>();
        }

        public string Id { get; set; }
        public string NormalizedProjectName { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public string Name { get; set; }
        public ActivityType Type { get; set; }
        public ActivityState State { get; set; }
        public DateTime? EarlyStart { get; set; }
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
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public Project Project { get; set; }
        public Organization Organization { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Arrow> OutArrows { get; set; }
        public List<Arrow> InArrows { get; set; }

        public object Clone()
        {
            return new Activity
            {
                Name = this.Name,
                Priority = this.Priority,
                Description = this.Description,
                Duration = this.Duration,
                EarlyFinish = this.EarlyFinish,
                EarlyStart = this.EarlyStart,
                EstimatedHours = this.EstimatedHours,
                FreeFloat = this.FreeFloat,
                HoursSpent = this.HoursSpent,
                LateFinish = this.LateFinish,
                LateStart = this.LateStart,
                Resources = this.Resources,
                TotalFloat = this.TotalFloat,
                Type = this.Type,
                State = this.State,
                NormalizedOrganizationName = this.NormalizedOrganizationName,
                NormalizedProjectName = this.NormalizedProjectName
            };
        }
    }
}
