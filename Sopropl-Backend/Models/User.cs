using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Sopropl_Backend.Models
{
    public partial class User
    {
        public User()
        {
            this.DateCreated = DateTime.Now;
            this.Members = new Collection<Member>();
            this.Projects = new Collection<Project>();
            this.Notifications = new Collection<Notification>();
            this.Invitations = new Collection<Invitation>();
            // this.Ownerships = new Collection<OrganizationOwner>();
        }

        [PersonalData]
        public string Id { get; set; }
        public string NormalizedUserName { get; set; }
        [PersonalData]
        public string UserName { get; set; }
        public string NormalizedEmail { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string PostalCode { get; set; }
        [ProtectedPersonalData]
        public string Email { get; set; }
        [PersonalData]
        public bool EmailConfirmed { get; set; }
        [ProtectedPersonalData]
        public string PhoneNumber { get; set; }
        [PersonalData]
        public bool PhoneNumberConfirmed { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        [PersonalData]
        public bool TwoFactorEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public ICollection<Member> Members { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        // public ICollection<OrganizationOwner> Ownerships { get; set; }
        public string PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}