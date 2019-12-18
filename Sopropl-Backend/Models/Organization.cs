using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sopropl_Backend.Models
{
    public partial class Organization
    {
        public Organization()
        {
            this.Members = new Collection<Member>();
            this.Projects = new Collection<Project>();
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Invitations = new Collection<Invitation>();
            // this.OrganizationOwners = new Collection<OrganizationOwner>();
        }

        public string Id { get; set; }
        public string NormalizedName { get; set; }
        public string Name { get; set; }
        public string LogoId { get; set; }
        public Photo Logo { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsBeta { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeactivated { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public ICollection<Member> Members { get; set; }
        public ICollection<Project> Projects { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        // public ICollection<OrganizationOwner> OrganizationOwners { get; set; }
    }
}
