using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly SoproplDbContext context;
        private readonly ITeamRepository teamRepo;
        public MemberRepository(SoproplDbContext context, ITeamRepository teamRepo)
        {
            this.teamRepo = teamRepo;
            this.context = context;
        }

        public async Task<Member> CreateAsync(Organization organization, User user)
        {
            var member = await this.FindMemberInOrganizationAsync(organization, user);
            if (member != null)
            {
                return member;
            }
            member = new Member { Organiztion = organization, User = user };
            this.context.Add(member);
            return member;
        }

        public async Task<Member> FindMemberInOrganizationAsync(Organization organization, User user)
        {
            var member = await this.context.Members.FirstOrDefaultAsync(m => m.NormalizedOrganizationName == organization.NormalizedName && m.NormalizedUserName == user.NormalizedUserName);

            return member;
        }

        public async Task<Member> FindMemberInTeamAsync(Organization organization, Team team, string memberId)
        {
            var member = await this.context.Members.FirstOrDefaultAsync(m => m.NormalizedOrganizationName == organization.Id && m.NormalizedTeamName == team.Id && m.Id == memberId);

            return member;
        }
    }
}