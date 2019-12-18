using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface IInvitationRepository
    {
        Task<bool> Invite(User inviter, User invitedUser, Organization organization);
        Task<bool> RemoveInvitationAsync(User invitedUser, Organization organization);
        Task<IEnumerable<User>> AllInvitedUsersAsync(Organization organization);
        Task<bool> AcceptInvitation(User invitedUserId, Organization organization);
        Task<Invitation> FindInvitationAsync(Organization organization, User invitedUser);

        Task<bool> IsInvitationExist(User invitedUser, Organization organization);

        Task<bool> SaveChangesAsync();
    }
}