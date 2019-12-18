using System;

namespace Sopropl_Backend.DTOs
{
    public class ProjectToReturnDTO
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public PhotoToReturnDTO Logo { get; set; }
    }
}