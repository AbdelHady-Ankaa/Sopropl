using System.Threading.Tasks;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface ISmtpClientRepository
    {
        Task<bool> SendEmail(string to, string body, string subject = "");

        Task<bool> SendInvitationToJoinToOrganization(User inviter, User invitedUser, Organization organization);

        // Task<bool> SendInvitationToJoinToOrganizationInWithinTeam(User inviter, User invitedUser, Organization organization, Team team);
    }
}