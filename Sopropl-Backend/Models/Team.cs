using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sopropl_Backend.Models
{
    public class Team
    {
        public Team()
        {
            this.Tasks = new Collection<Resource>();
            this.Members = new Collection<Member>();
            this.AccessList = new Collection<Access>();
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
        }
        public string Id { get; set; }
        public string NormalizedName { get; set; }
        public string Name { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public ICollection<Member> Members { get; set; }
        public Organization Organization { get; set; }
        public ICollection<Resource> Tasks { get; set; }
        public ICollection<Access> AccessList { get; set; }
    }
}