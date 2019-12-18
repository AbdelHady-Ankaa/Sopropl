using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class SmtpClientRespository : ISmtpClientRepository
    {
        private readonly IConfiguration Configuration;
        public SmtpClientRespository(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }



        public async Task<bool> SendEmail(string to, string body, string subject = "")
        {
            return await this.SendEmailMessage(to, body, subject);
        }

        public Task<bool> SendInvitationToJoinToOrganization(User inviter, User invitedUser, Organization organization)
        {
            var subject = $"{inviter.UserName} has invited you to join at {organization.Name} organization";
            var body = $"{inviter.UserName} has invited you to join at {organization.Name} organization";
            return this.SendEmailMessage(invitedUser.Email, body, subject);
        }

        // public Task<bool> SendInvitationToJoinToOrganizationInWithinTeam(User inviter, User invitedUser, Organization organization, Team team)
        // {
        //     var subject = $"{inviter.UserName} has invited you to join at {organization.Name} organization";
        //     var body = $"{inviter.UserName} has invited you to join at {organization.Name} organization within {team.NormalizedName} team";
        //     return this.SendEmailMessage(invitedUser.Email, body, subject);
        // }

        private async Task<bool> SendEmailMessage(string to, string body, string subject = "")
        {
            var message = new MailMessage();
            message.From = new MailAddress(Configuration.GetSection("Email:Smtp:Username").Value);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = Configuration.GetSection("Email:Smtp:Host").Value;
                smtpClient.Port = int.Parse(Configuration.GetSection("Email:Smtp:Port").Value);
                var username = Configuration.GetSection("Email:Smtp:Username").Value;
                var password = Configuration.GetSection("Email:Smtp:Password").Value;
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);

            }
            return true;
        }
    }
}