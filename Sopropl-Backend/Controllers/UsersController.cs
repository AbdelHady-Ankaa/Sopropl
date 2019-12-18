using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Sopropl_Backend.Data;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;
using Sopropl_Backend.SignalRHubs;

namespace Sopropl_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [AuthenticateAttribute]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepo;
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepo;
        private readonly IMemberRepository memberRepo;
        private readonly ISmtpClientRepository smtpClientRepo;
        private readonly INotificationRepository notificationRepo;
        private readonly IInvitationRepository invitationRepo;

        public UsersController(
        IUserRepository userRepo,
        IMapper mapper,
        IOrganizationRepository organizationRepo,
        IMemberRepository memberRepo,
        ISmtpClientRepository smtpClientRepo,
        INotificationRepository notificationRepo,
        IInvitationRepository invitationRepo)
        {
            this.memberRepo = memberRepo;
            this.smtpClientRepo = smtpClientRepo;
            this.notificationRepo = notificationRepo;
            this.invitationRepo = invitationRepo;
            this.organizationRepo = organizationRepo;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string userName)
        {
            if (HttpContext.Items.ContainsKey("current-user"))
            {
                var user = HttpContext.Items["current-user"] as User;
                var users = await this.userRepo.SearchAsync(user, userName);
                var usersToReturn = this.mapper.Map<IEnumerable<UserToReturnDTO>>(users);
                return Ok(usersToReturn);
            }
            return Unauthorized();
        }



        [HttpPost]
        [Route("acceptInvitation")]
        public async Task<IActionResult> AcceptInvitation([FromQuery]string organizationId)
        {
            var invitedUser = (User)HttpContext.Items["current-user"] as User;

            var organization = await this.organizationRepo.FindByIdAsync(organizationId);
            if (organization != null)
            {
                if (await this.invitationRepo.AcceptInvitation(invitedUser, organization))
                {
                    if (await this.invitationRepo.SaveChangesAsync())
                    {
                        return Ok(new { Message = $"You are now a member in @{organization.Name} organization" });
                    }
                }
                return BadRequest("You do not have any invitation to join this organization or the invitation has been canceled");
            }
            return NotFound("organization not exist");
        }
    }
}