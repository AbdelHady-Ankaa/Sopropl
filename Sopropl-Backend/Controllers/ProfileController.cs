using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository userRepo;
        private readonly IPhotoRepository photoRepo;
        private readonly IMapper mapper;
        public ProfileController(IUserRepository userRepo, IPhotoRepository photoRepo, IMapper mapper)
        {
            this.mapper = mapper;
            this.photoRepo = photoRepo;
            this.userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await this.userRepo.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
            if (user != null && user.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                var userProfile = this.mapper.Map<UserProfileDTO>(user);
                return Ok(userProfile);
            }
            return Unauthorized();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDTO userProfile)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userRepo.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
                if (user != null && user.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var newUserProfile = this.mapper.Map<User>(userProfile);
                    this.userRepo.UpdateProfile(user, newUserProfile);
                    if (await this.userRepo.SaveChangesAsync())
                    {
                        userProfile = this.mapper.Map<UserProfileDTO>(user);
                        return Ok(userProfile);
                    }
                    return BadRequest("Could not update the user profile");
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }

        [Route("setPhoto")]
        [HttpPost]
        public async Task<IActionResult> SetUserPhoto([FromForm]PhotoForCreationDTO photoForCreation)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userRepo.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
                if (user != null && user.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    if (user.Photo == null)
                    {
                        user.Photo = new Photo { User = user };
                        this.photoRepo.AddPhotoFile(user.Photo, photoForCreation.File);
                    }
                    else
                    {
                        this.photoRepo.UpdatePhotoFile(user.Photo, photoForCreation.File);
                    }
                    if (await this.photoRepo.SaveChangesAsync())
                    {
                        var photoToReturn = this.mapper.Map<PhotoToReturnDTO>(user.Photo);
                        return Ok(photoToReturn);
                    }
                    return BadRequest("Could not set the user photo");
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }
        [Route("deletePhoto")]
        [HttpDelete]
        public async Task<IActionResult> deletePhoto()
        {
            var user = await this.userRepo.FindByNameAsync(User.FindFirst(ClaimTypes.Name).Value);
            if (user != null)
            {
                if (user.Photo != null && user.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    this.photoRepo.Remove(user.Photo);
                    if (await this.photoRepo.SaveChangesAsync())
                    {
                        return Ok();
                    }
                    return BadRequest("Could not delete the user photo");
                }
                return NotFound("you are not have photo profile");
            }
            return Unauthorized();
        }
    }
}