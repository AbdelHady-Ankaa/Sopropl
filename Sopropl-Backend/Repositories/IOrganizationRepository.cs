using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface IOrganizationRepository
    {
        Task<bool> CreateAsync(User Owner, Organization organization);
        Task RemoveAsync(Organization organization);
        Task<Organization> FindByNameAsync(string organizationName);
        Task UpdateWithNameAsync(Organization oldOrganization, Organization newOrganization);
        void UpdateWithoutName(Organization oldOrganization, Organization newOrganization);
        Task<Member> AddMemberAsync(Organization organization, User user, short type = @short.MEMBER);
        void RemoveMember(Organization organization, Member member);
        Task<Member> FindMemberAsync(Organization organization, User user);
        Organization FindByName(string organizationName);
        Task<IEnumerable<Organization>> AllForMembersAsync(User user);
        Task<IEnumerable<Organization>> AllForOwnersAsync(User user);
        Task<Organization> FindByIdAsync(string organizationId);
        Task<IEnumerable<User>> AllMembersAsync(Organization organization, short type = @short.OTHER);
        Task<IEnumerable<Member>> AllOwnersAsync(Organization organization);
        Task<bool> SaveChangesAsync();
        void SetMemberRole(Organization organization, Member member, short role);
    }
}