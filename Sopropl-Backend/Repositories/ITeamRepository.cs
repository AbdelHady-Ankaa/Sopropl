using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> AllTeamsAsync(Organization organization);
        Task<Team> FindByIdAsync(Organization organization, string teamId);
        Task<Team> FindByNameAsync(Organization organization, string teamName);
        Task<bool> AddMemberAsync(Organization organization, Team team, User user);
        Task<Member> FindMemberAsync(Organization organization, Team team, string userName);
        Task<bool> RemoveMemberAsync(Organization organization, Team team, User user);
        Task<IEnumerable<User>> AllMembersAsync(Organization organization, string teamName);
        Task<Team> CreateAsync(Organization organization, Team team);
        Task<bool> RemoveAsync(Organization organization, string teamName);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Access>> AllPermessionsAsync(Organization organization, Team team, short accessType = AccessType.OTHER);
    }
}