using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Controllers
{
    [Authorize]
    [Route("api/photos")]
    [ApiController]
    // [AuthenticateFilter]
    public class PhotosController : ControllerBase
    {
        private readonly IUserRepository userRepo;
        private readonly IPhotoRepository photoRepo;
        private readonly IMapper mapper;
        private readonly IProjectRepository projectRepo;
        private readonly IOrganizationRepository organizationRepo;
        public PhotosController(IUserRepository userRepo, IProjectRepository projectRepo, IOrganizationRepository organizationRepo, IPhotoRepository photoRepo, IMapper mapper)
        {
            this.organizationRepo = organizationRepo;
            this.projectRepo = projectRepo;
            this.mapper = mapper;
            this.photoRepo = photoRepo;
            this.userRepo = userRepo;
        }

        
        
        

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(string id)
        {
            var photo = await this.photoRepo.FindByIdAsync(id);
            if (photo != null)
            {
                var photoToReturn = this.mapper.Map<PhotoToReturnDTO>(photo);
                return Ok(photoToReturn);
            }
            return NotFound();
        }
    }
}