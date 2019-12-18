using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface INotificationRepository
    {
        Task<bool> RemoveAsync(User user, Notification notification);
        Task<bool> RemoveAsync(User user, Organization organization);
        Task sendNoificationToUser(User toUser, string body, bool storeIt, string title, short type = NotificationType.NORMAL, string data = null);

        Task sendInvitationToJoinToOrganization(User inviter, User InvitedUser, Organization organization, bool storeIt = false);

        Task SendInvitationToJoinToOrganizationInWithinTeam(User inviter, User invitedUser, Organization organization, Team team, bool storeIt = false);

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Notification>> getUserNotifications(User user);
    }
}