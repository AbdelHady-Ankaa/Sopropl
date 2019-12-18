using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sopropl_Backend.Data;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager authRepository;
        private readonly IMapper mapper;
        public AuthController(IAuthManager authRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.authRepository = authRepository;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserForLoginDTO userForLoginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await this.authRepository.Login(userForLoginDTO.UserName, userForLoginDTO.Password);
                if (user == null)
                {
                    return Unauthorized();
                }
                var tokenString = this.authRepository.GenerateToken(user);
                var userToReturn = this.mapper.Map<UserToReturnDTO>(user);
                return Ok(new { tokenString, user = userToReturn });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDTO userForLoginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = this.mapper.Map<User>(userForLoginDTO);
                user = await this.authRepository.Register(user, userForLoginDTO.Password);
                if (user == null)
                {
                    ModelState.AddModelError("UserName", "UserName is already taken");
                }
                else
                {
                    var userToReturn = this.mapper.Map<UserToReturnDTO>(user);
                    return Ok(userToReturn);
                }
            }
            return BadRequest(ModelState);
        }
    }
}