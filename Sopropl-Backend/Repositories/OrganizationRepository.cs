using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly SoproplDbContext context;
        private readonly INormalizer<string> nameNormalizer;
        private readonly IProjectRepository projectRepo;
        public OrganizationRepository(SoproplDbContext context, IProjectRepository projectRepo)
        {
            this.projectRepo = projectRepo;
            this.nameNormalizer = new NameNormalizer();
            this.context = context;
        }
        public async Task<bool> CreateAsync(User Owner, Organization organization)
        {
            var o = await this.FindByNameAsync(organization.Name);
            if (o == null)
            {
                organization.NormalizedName = this.nameNormalizer.Normalize(organization.Name);
                organization.Members.Add(new Member { User = Owner, Type = @short.OWNER });

                this.context.Organizations.Add(organization);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Organization>> AllForOwnersAsync(User user)
        {
            var memberhipes = await this.context.Members
                .Include(o => o.Organiztion)
                .Where(m => m.NormalizedUserName == user.NormalizedUserName && m.Type == @short.OWNER)
                .ToListAsync();

            var organizations = new List<Organization>();

            memberhipes.ForEach(m =>
            {
                organizations.Add(m.Organiztion);
            });

            return organizations;
        }

        public Organization FindByName(string name)
        {
            name = this.nameNormalizer.Normalize(name);
            var org = this.context.Organizations.FirstOrDefault(o => o.NormalizedName == name);

            return org;
        }

        public async Task<Organization> FindByNameAsync(string name)
        {
            name = this.nameNormalizer.Normalize(name);
            var org = await this.context.Organizations.Include(o => o.Logo).FirstOrDefaultAsync(o => o.NormalizedName == name);

            return org;
        }
        private async Task<Organization> FindByNameIncludeAllAsync(string name)
        {
            name = this.nameNormalizer.Normalize(name);
            var org = await this.context.Organizations
            .Include(o => o.Projects)
            .ThenInclude(p => p.Activities)
            .ThenInclude(a => a.OutArrows)
            .Include(o => o.Members)
            .Include(o => o.Teams)
            .ThenInclude(t => t.AccessList)
            .Include(o => o.Logo)
            .FirstOrDefaultAsync(o => o.NormalizedName == name);

            return org;
        }

        public async Task<IEnumerable<Organization>> AllForMembersAsync(User user)
        {
            var memberShips = await this.context.Members.Include(m => m.Organiztion).Where(m => m.NormalizedUserName == user.NormalizedUserName).ToListAsync();
            IDictionary<string, Organization> organizations = new Dictionary<string, Organization>();
            memberShips.ForEach(m =>
            {
                if (m.Organiztion != null)
                {
                    organizations.Add(m.Organiztion.Id, m.Organiztion);
                }
            });
            return organizations.Values;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<Organization> FindByIdAsync(string organizationId)
        {
            return await this.context.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);
        }

        public async Task RemoveAsync(Organization organization)
        {
            var projects = this.context.Projects.Where(p => p.NormalizedOrganizationName == organization.NormalizedName).ToList();
            foreach (var p in projects)
            {
                await this.projectRepo.RemoveAsync(organization, p);
            }
            this.context.Remove(organization);
        }

        public async Task<Member> AddMemberAsync(Organization organization, User user, short type = @short.MEMBER)
        {
            var member = await this.FindMemberAsync(organization, user);
            if (member != null)
            {
                return member;
            }
            member = new Member { Organiztion = organization, User = user, Type = type };
            this.context.Add(member);
            return member;
        }

        public async Task<Member> FindMemberAsync(Organization organization, User user)
        {
            var member = await this.context.Members.Include(m => m.Team)
            .FirstOrDefaultAsync(m => m.NormalizedOrganizationName == organization.NormalizedName && m.NormalizedUserName == user.NormalizedUserName);

            return member;
        }

        public void RemoveMember(Organization organization, Member member)
        {
            member.Tasks.RemoveAll(_ => true);
            this.context.Remove(member);
        }

        public async Task UpdateWithNameAsync(Organization oldOrganization, Organization newOrganization)
        {
            var organization = await this.FindByNameIncludeAllAsync(oldOrganization.Name);
            if (organization != null)
            {

                var projects = organization.Projects;
                var teams = organization.Teams;
                var log = organization.Logo;
                var members = organization.Members;
                await this.RemoveAsync(organization);
                if (await this.SaveChangesAsync())
                {
                    oldOrganization.NormalizedName = this.nameNormalizer.Normalize(newOrganization.Name);
                    oldOrganization.Name = newOrganization.Name;
                    oldOrganization.Website = newOrganization.Website;
                    oldOrganization.ContactPhone = newOrganization.ContactPhone;
                    oldOrganization.Projects = projects;
                    oldOrganization.Teams = teams;
                    oldOrganization.Logo = log;
                    oldOrganization.Members = members;
                    this.context.Add(oldOrganization);
                }
            }
        }

        public void UpdateWithoutName(Organization oldOrganization, Organization newOrganization)
        {
            oldOrganization.Website = newOrganization.Website;
            oldOrganization.ContactPhone = newOrganization.ContactPhone;
            this.context.Update(oldOrganization);
        }

        public async Task<IEnumerable<User>> AllMembersAsync(Organization organization, short type = @short.OTHER)
        {
            List<Member> members = new List<Member>();
            if (type == @short.OTHER)
            {
                members = await this.context.Members.Include(m => m.User)
                    .Where(m => m.NormalizedOrganizationName == organization.NormalizedName)
                    .ToListAsync();
            }
            if (type == @short.MEMBER || type == @short.OWNER)
            {
                members = await this.context.Members.Include(m => m.User)
                    .Where(m => m.NormalizedOrganizationName == organization.NormalizedName && m.Type == type)
                    .ToListAsync();
            }
            var users = new List<User>();
            foreach (var item in members)
            {
                users.Add(item.User);
            }

            return users;
        }

        public async Task<IEnumerable<Member>> AllOwnersAsync(Organization organization)
        {
            var owners = await this.context.Members
                .Where(m => m.NormalizedOrganizationName == organization.NormalizedName && m.Type == @short.OWNER)
                .ToListAsync();

            return owners;
        }

        public void SetMemberRole(Organization organization, Member member, short role)
        {
            member.Type = role;
        }
    }
}