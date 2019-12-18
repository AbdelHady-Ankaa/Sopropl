// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Sopropl_Backend.Repositories;
// using Microsoft.AspNet.OData;
// using Sopropl_Backend.DTOs;
// using System.Linq;
// using AutoMapper;
// using Sopropl_Backend.Models;
// using System;
// using System.Security.Claims;
// using Sopropl_Backend.Helpers;

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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [AuthenticateAttribute]
    [ValidateModel]

    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationRepository orgRepo;
        private readonly IUserRepository userRepo;
        private readonly IMapper mapper;
        private readonly IInvitationRepository invitationRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IPhotoRepository photoRepo;
        private readonly INormalizer<string> normalizer;
        public OrganizationsController(
            IOrganizationRepository orgRepo,
        IUserRepository userRepo,
        IPhotoRepository photoRepo,
        IMapper mapper,
        IInvitationRepository invitationRepo,
        IMemberRepository memberRepo)
        {
            this.photoRepo = photoRepo;
            this.mapper = mapper;
            this.invitationRepo = invitationRepo;
            this.memberRepo = memberRepo;
            this.userRepo = userRepo;
            this.orgRepo = orgRepo;
            this.normalizer = new NameNormalizer();
        }

        [HttpGet("{orgName}", Name = "GetOrganization")]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]

        public IActionResult GetOne(string orgName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                var orgToReturn = this.mapper.Map<OrganizationToReturnDTO>(org);
                return Ok(orgToReturn);
            }
            return NotFound();
        }

        [HttpGet]
        // [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> GetAll()
        {
            if (HttpContext.Items.ContainsKey("current-user"))
            {
                var user = HttpContext.Items["current-user"] as User;
                var organizations = await this.orgRepo.AllForMembersAsync(user);
                var organizationsToRerurn = this.mapper.Map<IEnumerable<OrganizationToReturnDTO>>(organizations);
                return Ok(organizationsToRerurn);
            }
            return Unauthorized();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrganizationForCreationDTO orgForCreateDTO)
        {
            var user = HttpContext.Items["current-user"] as User;
            var newOrg = this.mapper.Map<Organization>(orgForCreateDTO);

            if (await this.orgRepo.CreateAsync(user, newOrg))
            {
                var orgToReturn = this.mapper.Map<OrganizationToReturnDTO>(newOrg);
                if (await this.orgRepo.SaveChangesAsync())
                {
                    return CreatedAtRoute("GetOrganization",
                    new { controller = "Organizations", orgName = newOrg.Name },
                    new { message = $"@{newOrg.Name} organizaiton has been created successfully" });
                }
                return BadRequest($"Failed to create {orgForCreateDTO.Name} organization");
            }
            return BadRequest($"@{orgForCreateDTO.Name} organzization name is already taken");

        }

        [HttpPut("{orgName}")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> Update(string orgName, [FromBody]OrganizationForCreationDTO orgForUpdate)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var oldOrg = HttpContext.Items["organization"] as Organization;

                if (oldOrg.Id == orgForUpdate.Id)

                {
                    var newOrg = this.mapper.Map<Organization>(orgForUpdate);
                    if (oldOrg.NormalizedName == this.normalizer.Normalize(orgName))
                    {
                        this.orgRepo.UpdateWithoutName(oldOrg, newOrg);
                        if (await this.orgRepo.SaveChangesAsync())
                        {
                            return Ok(new { message = $"@{orgName} organization has been updated successfully" });
                        }
                        else
                        {
                            return BadRequest("Organization can't be modified");
                        }
                    }
                    await this.orgRepo.UpdateWithNameAsync(oldOrg, newOrg);
                    if (await this.orgRepo.SaveChangesAsync())
                    {
                        return Ok(new { message = $"@{orgName} organization has been updated successfully" });
                    }
                    return BadRequest("Organization can't be modified");
                }
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }

        [HttpDelete("{orgName}")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> Remove(string orgName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    await this.orgRepo.RemoveAsync(org);
                    if (await this.orgRepo.SaveChangesAsync())
                    {
                        return Ok(new { message = $"@{orgName} organization has been removed successfully" });
                    }
                    return BadRequest($"Failed to delete @{orgName} organization");
                }
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }

        [Route("{orgName}/setLogo")]
        [HttpPost]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> SetLogo(string orgName, [FromForm] PhotoForCreationDTO photoForCreation)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org.Logo == null)
                {
                    org.Logo = new Photo { Organization = org };
                    this.photoRepo.AddPhotoFile(org.Logo, photoForCreation.File);
                }
                else
                {
                    this.photoRepo.UpdatePhotoFile(org.Logo, photoForCreation.File);
                }
                if (await this.photoRepo.SaveChangesAsync())
                {
                    var photoToReturn = this.mapper.Map<PhotoToReturnDTO>(org.Logo);
                    return Ok(photoToReturn);
                }
                return BadRequest("Could not set the organization logo");
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }


        [Route("{orgName}/deleteLogo")]
        [HttpDelete]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> RemoveLogo(string orgName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org.Logo != null)
                {
                    this.photoRepo.Remove(org.Logo);
                    if (await this.photoRepo.SaveChangesAsync())
                    {
                        return Ok(new { message = "logo has been deleted successfully" });
                    }
                }
                return BadRequest("Could not delete the organization logo");
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }

        [HttpGet("allInvitedUsers")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> AllInvitedUsers(string orgName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                var invitedUsers = await this.invitationRepo.AllInvitedUsersAsync(org);
                var usersToReturn = this.mapper.Map<IEnumerable<User>>(invitedUsers);
                return Ok(usersToReturn);
            }
            return Unauthorized();
        }

        [HttpPost("{orgName}/invite")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> InviteUser(string orgName, string invitedUserName)
        {

            if (HttpContext.Items.ContainsKey("organization"))
            {
                if (HttpContext.Items.ContainsKey("current-user"))
                {
                    var inviter = HttpContext.Items["current-user"] as User;
                    var organization = HttpContext.Items["organization"] as Organization;

                    var invitedUser = await this.userRepo.FindByNameAsync(invitedUserName);
                    if (invitedUser != null)
                    {
                        if (await this.invitationRepo.Invite(inviter, invitedUser, organization))
                        {
                            if (await this.invitationRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = $"@{invitedUserName} has been invited" });
                            }
                            return BadRequest($"Failed to Invite {invitedUserName} to @{orgName} organization");
                        }
                        return BadRequest($"{invitedUserName} is already member in @{orgName}");
                    }
                    return NotFound($"{invitedUserName} isn't a Sopropl member");
                }
                return Unauthorized();
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }

        [HttpDelete("{orgName}/removeInvitation")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> removeInvitation(string orgName, [FromQuery]string invitedUserName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var organization = HttpContext.Items["organization"] as Organization;

                var invitedUser = await this.userRepo.FindByNameAsync(invitedUserName);

                if (organization != null)
                {
                    if (invitedUser != null)
                    {
                        if (await this.invitationRepo.RemoveInvitationAsync(invitedUser, organization))
                        {
                            if (await this.invitationRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = "Invitation has been removed successfully" });
                            }
                            return BadRequest($"Failed to delete the invitation from @{orgName} organization to {invitedUserName}");
                        }
                        return BadRequest($"There is no invitation for {invitedUserName}");
                    }
                    return NotFound($"{invitedUserName} isn't a Sopropl member");
                }
                return Unauthorized($"you don't have permissions to do this action");
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }


        [HttpDelete("{orgName}/deleteMember")]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> RemoveMember(string orgName, [FromQuery]string userName)
        {
            if (userName != null)
            {
                if (HttpContext.Items.ContainsKey("organization") && HttpContext.Items.ContainsKey("member"))
                {
                    var currentMember = HttpContext.Items["member"] as Member;
                    var org = HttpContext.Items["organization"] as Organization;
                    if (currentMember.NormalizedUserName == this.normalizer.Normalize(userName))
                    {
                        var owners = await this.orgRepo.AllOwnersAsync(org);
                        if (owners.Count() > 1)
                        {
                            this.orgRepo.RemoveMember(org, currentMember);
                            if (await this.orgRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = $"you are now not a member in @{orgName} organization" });
                            }
                            return BadRequest($"Failed to delete {userName} from @{orgName} organization");
                        }
                        else
                        {
                            return BadRequest($"you can't delete yourself from organization if you are " +
                            $"only owner for it if you don't need @{orgName} organization you can deleted");
                        }
                    }
                    var userMemberToRemove = await this.userRepo.FindByNameAsync(userName);
                    if (userMemberToRemove != null)
                    {
                        var memberToRemove = await this.orgRepo.FindMemberAsync(org, userMemberToRemove);
                        if (memberToRemove != null)
                        {
                            if (currentMember.Type > memberToRemove.Type)
                            {
                                this.orgRepo.RemoveMember(org, memberToRemove);
                                if (await this.orgRepo.SaveChangesAsync())
                                {
                                    return Ok(new { message = $"{userName} member has been deleted from @{orgName} organization successfully" });
                                }
                                return BadRequest($"Failed to delete {userName} from @{orgName} organization");
                            }
                            else
                            {
                                return BadRequest("An owner can not delete another owner");
                            }
                        }
                    }
                    return BadRequest($"{userName} is not a member in @{orgName} organization");
                }
                return Unauthorized($"you don't have permissions to do this action");
            }
            return BadRequest("some data are messing");
        }


        [HttpGet]
        [Route("{orgName}/AllMembers")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> AllMembers(string orgName, [FromQuery]short? role)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    if (role == null)
                    {
                        var users = await this.orgRepo.AllMembersAsync(org);
                        var usersToReturn = this.mapper.Map<IEnumerable<UserToReturnDTO>>(users);
                        return Ok(usersToReturn);
                    }
                    else
                    {
                        if (role.Value == @short.OWNER || role.Value == @short.MEMBER)
                        {
                            var users = await this.orgRepo.AllMembersAsync(org, role.Value);
                            var usersToReturn = this.mapper.Map<IEnumerable<UserToReturnDTO>>(users);
                            return Ok(usersToReturn);
                        }
                        else
                        {
                            return BadRequest($"not vaild role");
                        }

                    }

                }
            }
            return NotFound($"@{orgName} organizaiton isn't exist");
        }

        [Route("{orgName}/SetMemberRole")]
        [HttpPost]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> SetMemberRole(string orgName, [FromQuery]string userName, [FromQuery]short role)
        {
            if (role == @short.OWNER || role == @short.MEMBER)
            {
                if (HttpContext.Items.ContainsKey("organization"))
                {
                    var org = HttpContext.Items["organization"] as Organization;
                    if (org != null)
                    {
                        var user = await this.userRepo.FindByNameAsync(userName);
                        if (user != null)
                        {
                            var member = await this.orgRepo.FindMemberAsync(org, user);
                            if (member != null)
                            {
                                if (member.Type == @short.OWNER && role == @short.MEMBER)
                                {
                                    var owners = await this.orgRepo.AllOwnersAsync(org);
                                    if (owners.Count() > 1)
                                    {
                                        this.orgRepo.SetMemberRole(org, member, role);
                                        if (await this.orgRepo.SaveChangesAsync())
                                        {
                                            return Ok(new { message = $"you are now normal member in @{orgName} organizaiton" });
                                        }
                                        return BadRequest($"Failed to set yourself as normal member");
                                    }
                                    return BadRequest($"you can't set yourself as normal member if you are " +
                                    $"only owner for {orgName} organization");
                                }
                                else
                                {
                                    if (role == @short.OWNER || role == @short.MEMBER)
                                    {
                                        this.orgRepo.SetMemberRole(org, member, role);
                                        if (await this.orgRepo.SaveChangesAsync())
                                        {
                                            return Ok(new { message = $"permessions has been updated successfuly for @{userName} in @{orgName} organizaiton" });
                                        }
                                        return BadRequest($"Failed to change permessions for @{userName} in @{orgName} organization");
                                    }
                                }
                            }
                            return BadRequest($"{userName} is not a member in @{orgName} organization");
                        }
                    }
                }
                return NotFound($"@{orgName} organizaiton isn't exist");

            }
            return BadRequest($"not vaild role");


        }
    }
}