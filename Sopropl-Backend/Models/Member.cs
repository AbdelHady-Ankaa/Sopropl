using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sopropl_Backend.Models
{
    public partial class Member
    {
        public Member()
        {
            this.Tasks = new List<Resource>();
        }

        public string Id { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedTeamName { get; set; }
        public short Type { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();


        public Organization Organiztion { get; set; }
        public Team Team { get; set; }
        public User User { get; set; }
        public List<Resource> Tasks { get; set; }
    }
}
