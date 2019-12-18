using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly INotificationRepository notificationRepo;
        private readonly ISmtpClientRepository smtpClientRepo;
        private readonly IOrganizationRepository orgRepo;

        private readonly SoproplDbContext context;
        public InvitationRepository(SoproplDbContext context, IOrganizationRepository orgRepo, INotificationRepository notificationRepo, ISmtpClientRepository smtpClientRepo)
        {
            this.orgRepo = orgRepo;
            this.smtpClientRepo = smtpClientRepo;
            this.notificationRepo = notificationRepo;
            this.context = context;
        }

        public async Task<bool> AcceptInvitation(User invitedUser, Organization organization)
        {
            if (await this.RemoveInvitationAsync(invitedUser, organization))
            {
                var member = await this.orgRepo.AddMemberAsync(organization, invitedUser);
                return true;
            }
            return false;
        }

        public async Task<Invitation> FindInvitationAsync(Organization organization, User invitedUser)
        {
            var invitation = await this.context.Invitations.FirstOrDefaultAsync(i => i.NormalizedUserName == invitedUser.NormalizedUserName && i.NormalizedOrganizationName == organization.NormalizedName);

            return invitation;
        }

        public async Task<IEnumerable<User>> AllInvitedUsersAsync(Organization organization)
        {
            var invitations = await this.context.Invitations.Include(i => i.User)
                .Where(i => i.NormalizedOrganizationName == organization.NormalizedName)
                .ToListAsync();
            List<User> invitedUsers = new List<User>();
            foreach (var item in invitations)
            {
                invitedUsers.Add(item.User);
            }
            return invitedUsers;
        }

        public async Task<bool> Invite(User inviter, User invitedUser, Organization organization)
        {
            var member = await this.orgRepo.FindMemberAsync(organization, invitedUser);
            if (member == null)
            {
                var isInvitationExist = await this.IsInvitationExist(invitedUser, organization);
                if (!isInvitationExist)
                {
                    this.context.Invitations.Add(new Invitation { User = invitedUser, Organization = organization });
                    await this.notificationRepo.sendInvitationToJoinToOrganization(inviter, invitedUser, organization, !isInvitationExist);
                    await this.smtpClientRepo.SendInvitationToJoinToOrganization(inviter, invitedUser, organization);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> IsInvitationExist(User invitedUser, Organization organization)
        {
            var exist = await this.context.Invitations.AnyAsync(i => i.NormalizedUserName == invitedUser.Id && i.NormalizedOrganizationName == organization.NormalizedName);

            return exist;
        }

        public async Task<bool> RemoveInvitationAsync(User invitedUser, Organization organization)
        {
            var invitation = await this.FindInvitationAsync(organization, invitedUser);

            if (invitation == null)
            {
                return false;
            }
            await this.notificationRepo.RemoveAsync(invitedUser, organization);
            this.context.Invitations.Remove(invitation);
            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}