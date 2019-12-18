using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Authenticate]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository userRepo;
        private readonly IPhotoRepository photoRepo;
        private readonly IMapper mapper;
        private readonly INotificationRepository notificationRepo;
        public AccountController(IUserRepository userRepo, IPhotoRepository photoRepo, IMapper mapper, INotificationRepository notificationRepo)
        {
            this.notificationRepo = notificationRepo;
            this.mapper = mapper;
            this.photoRepo = photoRepo;
            this.userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await this.userRepo.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
            if (user != null && user.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                var userToReturn = this.mapper.Map<UserToReturnDTO>(user);
                return Ok(userToReturn);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("getNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var user = (User)HttpContext.Items.Single(i => i.Key as string == "current-user").Value;

            var notifications = await this.notificationRepo.getUserNotifications(user);

            var notificationsToReturn = this.mapper.Map<IEnumerable<NotificationDTO>>(notifications);

            return Ok(notificationsToReturn);
        }
    }
}