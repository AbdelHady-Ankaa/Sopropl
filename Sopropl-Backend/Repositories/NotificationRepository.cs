using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;
using Sopropl_Backend.SignalRHubs;

namespace Sopropl_Backend.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly IMapper mapper;
        private readonly SoproplDbContext context;
        public NotificationRepository(IHubContext<NotificationHub> hubContext, IMapper mapper, SoproplDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        public async Task<IEnumerable<Notification>> getUserNotifications(User user)
        {
            var notifications = await this.context.Notifications.Where(n => n.NormalizedUserName == user.NormalizedUserName).ToListAsync();

            return notifications;
        }

        public Task<bool> RemoveAsync(User user, Notification notification)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> RemoveAsync(User user, Organization organization)
        {
            var notification = await this.context.Notifications
            .FirstOrDefaultAsync(n =>
            n.NormalizedUserName == user.NormalizedUserName &&
            n.Type == NotificationType.INVITATION &&
            n.Data == organization.Id);
            if (notification != null)
            {
                this.context.Notifications.Remove(notification);
                return true;
            }
            return false;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task sendInvitationToJoinToOrganization(User inviter, User InvitedUser, Organization organization, bool storeIt = false)
        {
            var body = $"{inviter.UserName} has invited you to join at @{organization.Name} organization";

            await this.sendNoificationToUser(InvitedUser, body, storeIt, "Invitation", NotificationType.INVITATION, organization.Id);
        }

        public async Task SendInvitationToJoinToOrganizationInWithinTeam(User inviter, User invitedUser, Organization organization, Team team, bool storeIt = false)
        {
            var body = $"{inviter.UserName} has invited you to join at @{organization.Name} organization within {team.NormalizedName} team";
            await this.sendNoificationToUser(invitedUser, body, storeIt, "Invitation", NotificationType.INVITATION, organization.Id);
        }


        public async Task sendNoificationToUser(User toUser, string body, bool storeIt, string title, short type = NotificationType.NORMAL, string data = null)
        {
            var notification = new Notification { Body = body, Title = title, Data = data, Type = type };
            if (storeIt)
            {
                toUser.Notifications.Add(notification);
            }

            var notificationToSend = this.mapper.Map<NotificationDTO>(notification);
            await this.hubContext.Clients.User(toUser.Id).SendAsync("notify", notificationToSend);
        }
    }
}