using System;

namespace Sopropl_Backend.Models
{
    public class Arrow : ICloneable
    {
        public string NormalizedOrganizationName { get; set; }
        public string NormalizedProjectName { get; set; }
        public string FromActivityName { get; set; }
        public string ToActivityName { get; set; }
        public Activity FromActivity { get; set; }
        public Activity ToActivity { get; set; }
        public double Value { get; set; }
        public ArrowType Type { get; set; }

        public object Clone()
        {
            return new Arrow
            {
                Value = this.Value,
                Type = this.Type
            };
        }
    }
}