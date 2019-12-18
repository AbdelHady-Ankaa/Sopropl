using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sopropl_Backend.Models
{
    public partial class Project
    {
        public Project()
        {
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Activities = new Collection<Activity>();
            this.AccessList = new Collection<Access>();
        }

        public string Id { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public string NormalizedName { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LogoId { get; set; }
        public Photo Logo { get; set; }
        public string Description { get; set; }
        public string ProjectType { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string ClientId { get; set; }
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual User Client { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public ICollection<Access> AccessList { get; set; }
    }
}
