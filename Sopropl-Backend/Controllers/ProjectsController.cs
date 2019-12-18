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
    [Route("api/{orgName}/[controller]")]
    [Authorize]
    [AuthenticateAttribute]
    [ApiController]
    [ValidateModel]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository projectRepo;
        private readonly IOrganizationRepository orgRepo;
        private readonly IMapper mapper;
        private readonly ITeamRepository teamRepo;
        private readonly IUserRepository userRepo;
        private readonly IPhotoRepository photoRepo;
        private readonly INormalizer<string> normalizer;
        public ProjectsController(
            IProjectRepository projectRepo,
            IOrganizationRepository orgRepo,
            IPhotoRepository photoRepo,
            IMapper mapper,
            ITeamRepository teamRepo,
            IUserRepository userRepo)
        {
            this.photoRepo = photoRepo;
            this.mapper = mapper;
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.orgRepo = orgRepo;
            this.projectRepo = projectRepo;
            this.normalizer = new NameNormalizer();
        }

        [HttpPost]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> Create(string orgName, ProjectForCreationDTO projForCreate)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                var newProj = this.mapper.Map<Project>(projForCreate);
                if (await this.projectRepo.CreateAsync(org, newProj))
                {
                    if (await this.projectRepo.SaveChangesAsync())
                    {
                        return CreatedAtRoute("GetProject",
                        new { controller = "Projects", orgName = org.Name, projectName = newProj.Name },
                        new { message = $"{projForCreate.Name} has been created successfully" });
                    }
                    return BadRequest($"Failed to create {newProj.NormalizedName} organization");
                }
                return BadRequest($"{projForCreate.Name} already exist");
            }
            return NotFound();
        }

        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.CONTRIBUTOR, ParamType = ParamType.PATH_PARAM)]
        [HttpGet("{projectName}", Name = "GetProject")]
        public IActionResult GetOne(string orgName, string projectName)
        {
            if (HttpContext.Items.ContainsKey("project"))
            {
                var proj = HttpContext.Items["project"] as Project;
                if (proj != null)
                {
                    var projectToRetuen = this.mapper.Map<ProjectToReturnDTO>(proj);
                    return Ok(projectToRetuen);
                }
            }
            return NotFound($"@{projectName} isn't a project in @{orgName} organization");
        }

        [HttpGet]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        // [AuthAccessToProject(Role = AccessType.CONTRIBUTOR, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> GetAll()
        {
            var member = HttpContext.Items["member"] as Member;
            var org = HttpContext.Items["organization"] as Organization;
            IEnumerable<Project> projects;
            IEnumerable<ProjectToReturnDTO> projectsToReturn;
            if (member.Type == @short.OWNER)
            {
                projects = await this.projectRepo.AllForOwnerAsync(org, member);
                projectsToReturn = this.mapper.Map<IEnumerable<ProjectToReturnDTO>>(projects);
                return Ok(projectsToReturn);
            }
            projects = await this.projectRepo.AllForTeamAsync(org, member.Team);
            projectsToReturn = this.mapper.Map<IEnumerable<ProjectToReturnDTO>>(projects);
            return Ok(projectsToReturn);
        }

        [HttpPut("{projectName}")]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> Update(string projectName, string orgName, ProjectForCreationDTO projForUpdate)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    if (HttpContext.Items.ContainsKey("project"))
                    {
                        var oldProject = HttpContext.Items["project"] as Project;
                        var newProject = this.mapper.Map<Project>(projForUpdate);
                        if (oldProject.NormalizedName != this.normalizer.Normalize(newProject.Name))
                        {
                            await this.projectRepo.UpdateWithNameAsync(org, oldProject, newProject);
                            if (await this.projectRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = $"@{projectName} project has been updated successfully" });
                            }
                            return BadRequest($"Failed to update @{projectName} project");
                        }
                        else
                        {
                            this.projectRepo.UpdateWithoutName(org, oldProject, newProject);
                            if (await this.projectRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = $"@{projectName} project has been updated successfully" });
                            }
                            return BadRequest($"Failed to update @{projectName} project");
                        }
                    }
                    return NotFound($"{projectName} project isn't exist");
                }
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }

        [HttpDelete("{projectName}")]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> Remove(string orgName, string projectName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    var project = HttpContext.Items["project"] as Project;
                    if (project != null)
                    {
                        await this.projectRepo.RemoveAsync(org, project);
                        if (await this.projectRepo.SaveChangesAsync())
                        {
                            return Ok(new { message = $"{projectName} has been created successfully" });
                        }
                        return BadRequest($"Failed to delete {projectName} project");
                    }
                    return NotFound($"{projectName} project isn't exist");
                }

            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }


        [Route("{projectName}/setLogo")]
        [HttpPost]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> SetLogo(string orgName, string projectName, [FromForm] PhotoForCreationDTO photoForCreation)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    if (HttpContext.Items.ContainsKey("project"))
                    {
                        var project = HttpContext.Items["project"] as Project;
                        if (project != null)
                        {
                            if (project.Logo == null)
                            {
                                project.Logo = new Photo { Project = project };
                                this.photoRepo.AddPhotoFile(project.Logo, photoForCreation.File);
                            }
                            else
                            {
                                this.photoRepo.UpdatePhotoFile(project.Logo, photoForCreation.File);
                            }
                            if (await this.photoRepo.SaveChangesAsync())
                            {
                                var photoToReturn = this.mapper.Map<PhotoToReturnDTO>(project.Logo);
                                return Ok(photoToReturn);
                            }
                            return BadRequest("Could not set the project logo");
                        }
                    }
                    return NotFound($"{projectName} project isn't exist");
                }
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }
        [Route("{projectName}/deleteLogo")]
        [HttpDelete]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> RemoveLogo(string orgName, string projectName)
        {
            if (HttpContext.Items.ContainsKey("organization"))
            {
                var org = HttpContext.Items["organization"] as Organization;
                if (org != null)
                {
                    if (HttpContext.Items.ContainsKey("project"))
                    {
                        var project = HttpContext.Items["project"] as Project;
                        if (project != null)
                        {
                            this.photoRepo.Remove(project.Logo);
                            if (await this.photoRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = "logo has been deleted successfully" });
                            }
                            return BadRequest($"Could not delete the @{projectName} logo");
                        }
                    }
                    return NotFound($"{projectName} project isn't exist");
                }
            }
            return NotFound($"{orgName} organizaiton isn't exist");
        }

        [Route("{projectName}/changeAccess")]
        [HttpPost]
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> ChangeAccess(string orgName, string projectName, [FromQuery] string teamName, [FromQuery] string userName, [FromQuery] short type)
        {
            if (type == AccessType.MANAGER || type == AccessType.CONTRIBUTOR)
            {
                var org = HttpContext.Items["organization"] as Organization;
                var proj = HttpContext.Items["project"] as Project;
                if (teamName == null && userName != null)
                {
                    var userToAddToProject = await this.userRepo.FindByNameAsync(userName);
                    var member = await this.orgRepo.FindMemberAsync(org, userToAddToProject);
                    if (member != null)
                    {
                        if (member.Type == @short.OWNER)
                        {
                            return BadRequest("Can not add owner to team");
                        }
                        if (member.Team == null)
                        {
                            member.Team = new Team { Name = proj.Name };
                            member.Team = await this.teamRepo.CreateAsync(org, member.Team);
                            if (member.Team != null)
                            {
                                if (await this.projectRepo.ChangeAccessAsync(org, proj, member.Team, type))
                                {
                                    if (await this.projectRepo.SaveChangesAsync())
                                    {
                                        return Ok(new { message = $"@{userName} has been added successfully to @{projectName} and its default team" });
                                    }
                                }
                            }
                            return BadRequest($"Failed to add @{userName} to project");
                        }
                        return BadRequest($"@{userName} is a member in $@{member.Team.Name} team you can't add member " +
                        $"to project if you have Team if you need to add this member you can remove" +
                        $" it from @{member.Team.Name} team and add it to @{projectName} organization or you can add its team");
                    }
                    return BadRequest($"{userName} not a member in @{orgName} only @{orgName} members can be added to @{projectName}");
                }

                if (teamName != null && userName == null)
                {
                    var currentUser = HttpContext.Items["current-user"] as User;
                    if (currentUser.NormalizedUserName == this.normalizer.Normalize(userName))
                    {
                        return BadRequest("Can not add owner to team");
                    }
                    var team = await this.teamRepo.FindByNameAsync(org, teamName);
                    if (team != null)
                    {
                        if (await this.projectRepo.ChangeAccessAsync(org, proj, team, type))
                        {
                            if (await this.projectRepo.SaveChangesAsync())
                            {
                                return Ok(new { message = $"@{teamName} team has been added successfully to @{projectName}" });
                            }
                        }
                        return BadRequest($"Failed to add @{teamName} team to project");
                    }
                    return BadRequest($"{teamName} team isn't exist in {orgName} organization");
                }
            }

            return BadRequest("not valid paramters");
        }
    }
}