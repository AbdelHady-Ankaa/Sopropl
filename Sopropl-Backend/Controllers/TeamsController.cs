using System.Collections.Generic;
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
    [Authorize]
    [ApiController]
    [Route("api/{orgName}/[controller]")]
    [Authenticate]
    [ValidateModel]
    [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository teamRepo;
        private readonly IMapper mapper;
        private readonly IOrganizationRepository orgRepo;
        private readonly IUserRepository userRepo;
        private readonly INormalizer<string> normailzer;
        public TeamsController(ITeamRepository teamRepo, IMapper mapper, IOrganizationRepository orgRepo, IUserRepository userRepo)
        {
            this.userRepo = userRepo;
            this.orgRepo = orgRepo;
            this.mapper = mapper;
            this.teamRepo = teamRepo;
            this.normailzer = new NameNormalizer();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string orgName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    var teams = await this.teamRepo.AllTeamsAsync(org);

                    var teamsToReturn = this.mapper.Map<IEnumerable<TeamToReturnDTO>>(teams);
                    return Ok(teamsToReturn);
                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }
        [HttpGet("{teamName}", Name = "GetTeam")]
        public async Task<IActionResult> GetOne(string orgName, string teamName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    var team = await this.teamRepo.FindByNameAsync(org, teamName);
                    if (team != null)
                    {
                        var teamToReturn = this.mapper.Map<TeamToReturnDTO>(team);
                        return Ok(teamToReturn);
                    }
                    return NotFound($"{teamName} team isn't exist");
                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string orgName, [FromBody] TeamForCreateDTO teamForCreate)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                var newTeam = this.mapper.Map<Team>(teamForCreate);
                await this.teamRepo.CreateAsync(org, newTeam);
                if (await this.teamRepo.SaveChangesAsync())
                {
                    return CreatedAtRoute("GetTeam",
                    new { controller = "Teams", orgName = orgName, teamName = newTeam.Name },
                    new { message = $"@{newTeam.Name} team has been created successfully" });
                }
                return BadRequest($"Failed to create {teamForCreate.Name} team");
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }

        [HttpDelete("{teamName}")]
        public async Task<IActionResult> Remove(string orgName, string teamName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    if (await this.teamRepo.RemoveAsync(org, teamName))
                    {
                        if (await this.teamRepo.SaveChangesAsync())
                        {
                            return Ok(new { message = $"@{teamName} team has been removed successfully" });
                        }
                        return BadRequest($"Failed to delete {teamName} team");
                    }
                    return BadRequest($"{teamName} team isn't exist");
                }

            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }


        [HttpGet("{teamName}/allMembers")]
        public async Task<IActionResult> AllMembers(string orgName, string teamName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    var users = await this.teamRepo.AllMembersAsync(org, teamName);
                    var usersToReturn = this.mapper.Map<IEnumerable<UserToReturnDTO>>(users);
                    return Ok(usersToReturn);
                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }
        [HttpPost("{teamName}/addMember")]
        public async Task<IActionResult> AddMember(string orgName, string teamName, [FromQuery] string userName)
        {
            var currentUser = HttpContext.Items["current-user"] as User;
            if (this.normailzer.Normalize(userName) == currentUser.NormalizedUserName)
            {
                return BadRequest("organization owners can't be added to teams");
            }
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    var team = await this.teamRepo.FindByNameAsync(org, teamName);
                    if (team != null)
                    {
                        var user = await this.userRepo.FindByNameAsync(userName);
                        if (user != null)
                        {
                            if (await this.teamRepo.AddMemberAsync(org, team, user))
                            {
                                if (await this.teamRepo.SaveChangesAsync())
                                {
                                    return Ok(new { message = $"@{userName} has been added to @{teamName} team succesfully" });
                                }
                                return BadRequest($"Failed to add {userName} to @{teamName} team " +
                                "this may happen if it already member in team");
                            }
                            return BadRequest($"@{userName} isn't a @{orgName} member or it's an owner for it " +
                            $" you can't add member to team if it is not a member in team or it's an owner for orgnaztion");
                        }
                        return NotFound($"@{userName} isn't a Sopropl member");
                    }
                    return NotFound($"@{teamName} team isn't exist");
                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }


        [HttpDelete("{teamName}/deleteMember")]
        public async Task<IActionResult> RemoveMember(string orgName, string teamName, [FromQuery]string userName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {

                    var team = await this.teamRepo.FindByNameAsync(org, teamName);
                    if (team != null)
                    {
                        var user = await this.userRepo.FindByNameAsync(userName);
                        if (user != null)
                        {
                            if (await this.teamRepo.RemoveMemberAsync(org, team, user))
                            {
                                if (await this.teamRepo.SaveChangesAsync())
                                {
                                    return Ok(new { message = $"{userName} member has been deleted from @{teamName} team successfully" });
                                }
                                return BadRequest($"Failed to delete {userName} from @{teamName} team");
                            }
                            return BadRequest($"{userName} isn't a @{teamName} member");
                        }
                        return NotFound($"@{userName} isn't a Sopropl member");
                    }
                    return NotFound($"@{teamName} team isn't exist");
                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }

        [HttpGet("{teamName}/AllPermessions")]
        public async Task<IActionResult> AllPermessions(string orgName, string teamName, short? permession)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    var team = await this.teamRepo.FindByNameAsync(org, teamName);
                    if (team != null)
                    {
                        if (permession == null)
                        {
                            var permessions = await this.teamRepo.AllPermessionsAsync(org, team);
                            var accessListToReturn = this.mapper.Map<IEnumerable<AccessToReturnDTO>>(permessions);
                            return Ok(accessListToReturn);
                        }
                        {
                            var permessions = await this.teamRepo.AllPermessionsAsync(org, team, permession.Value);
                            var accessListToReturn = this.mapper.Map<IEnumerable<AccessToReturnDTO>>(permessions);
                            return Ok(accessListToReturn);
                        }
                    }
                    return NotFound($"{teamName} team isn't exist");
                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }
    }
}