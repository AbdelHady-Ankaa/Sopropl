using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SoproplDbContext context;
        private readonly IOrganizationRepository organizationRepo;
        private readonly INormalizer<string> normalizer;
        public TeamRepository(SoproplDbContext context, IOrganizationRepository organizationRepo)
        {
            this.organizationRepo = organizationRepo;
            this.context = context;
            this.normalizer = new NameNormalizer();
        }

        public async Task<bool> AddMemberAsync(Organization organization, Team team, User user)
        {
            var orgMember = await this.organizationRepo.FindMemberAsync(organization, user);
            if (orgMember != null)
            {
                if (orgMember.Type != @short.OWNER)
                {
                    orgMember.Team = team;
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Team>> AllTeamsAsync(Organization organization)
        {
            var teams = await this.context.Teams.Where(t => t.NormalizedOrganizationName == organization.NormalizedName).ToListAsync();

            return teams;
        }

        public async Task<Team> CreateAsync(Organization organization, Team team)
        {
            var t = await this.FindByNameAsync(organization, team.Name);
            if (t != null)
            {
                return t;
            }
            team.NormalizedName = this.normalizer.Normalize(team.Name);
            team.Organization = organization;
            this.context.Add(team);
            return team;
        }

        public async Task<Team> FindByIdAsync(Organization organization, string teamId)
        {
            var team = await this.context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == teamId && t.NormalizedOrganizationName == organization.NormalizedName);

            return team;
        }

        public async Task<bool> RemoveAsync(Organization organization, string teamName)
        {
            var team = await this.FindByNameAsync(organization, teamName);
            if (team != null)
            {
                this.context.Teams.Remove(team);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveMemberAsync(Organization organization, Team team, User user)
        {
            var member = await this.FindMemberAsync(organization, team, user.UserName);
            if (member != null)
            {
                member.Team = null;
                member.NormalizedTeamName = null;
                this.context.Update(member);
                return true;
            }
            return false;
        }

        public async Task<Team> FindByNameAsync(Organization organization, string teamName)
        {
            teamName = this.normalizer.Normalize(teamName);
            var team = await this.context.Teams.FirstOrDefaultAsync(t => t.NormalizedOrganizationName == organization.NormalizedName && t.NormalizedName == teamName);

            return team;
        }

        public async Task<IEnumerable<Access>> AllPermessionsAsync(Organization organization, Team team, short accessType = AccessType.OTHER)
        {
            if (accessType == AccessType.MANAGER || accessType == AccessType.CONTRIBUTOR)
            {
                var permissions = await this.context.AccessList
                .Where(a => a.NormalizedTeamName == team.NormalizedName && a.Permission == accessType)
                .ToListAsync();
                return permissions;
            }
            else
            {
                var permissions = await this.context.AccessList
                    .Where(a => a.NormalizedTeamName == team.NormalizedName)
                    .ToListAsync();
                return permissions;
            }
        }

        public async Task<Member> FindMemberAsync(Organization organization, Team team, string userName)
        {
            userName = this.normalizer.Normalize(userName);
            var member = await this.context.Members.Include(m => m.Team).FirstOrDefaultAsync(
                m => m.NormalizedOrganizationName == organization.NormalizedName &&
                m.NormalizedTeamName == team.NormalizedName &&
                m.NormalizedUserName == userName
            );

            return member;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> AllMembersAsync(Organization organization, string teamName)
        {
            teamName = this.normalizer.Normalize(teamName);
            var members = await this.context.Members.Include(m => m.User)
                .Where(
                m => m.NormalizedOrganizationName == organization.NormalizedName &&
                m.NormalizedTeamName == teamName).ToListAsync();
            var users = new List<User>();

            foreach (var item in members)
            {
                users.Add(item.User);
            }
            return users;
        }


    }
}