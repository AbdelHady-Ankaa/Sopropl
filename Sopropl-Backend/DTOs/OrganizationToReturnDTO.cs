using System;
using System.Collections.Generic;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.DTOs
{
    public class OrganizationToReturnDTO
    {
        public string Name { get; set; }
        public string LogoPublicId { get; set; }
        public PhotoToReturnDTO Logo { get; set; }
        public string ContactPhone { get; set; }
        public string Website { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public bool IsBeta { get; set; }
        public bool IsDeactivated { get; set; }
    }
}