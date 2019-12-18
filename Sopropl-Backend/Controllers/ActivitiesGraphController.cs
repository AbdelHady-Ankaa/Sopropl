using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sopropl_Backend.Repositories;
using Microsoft.AspNet.OData;
using Sopropl_Backend.DTOs;
using System.Linq;
using AutoMapper;
using Sopropl_Backend.Models;
using System;
using System.Security.Claims;
using Sopropl_Backend.Helpers;
using System.Collections.Generic;

namespace Sopropl_Backend.Controllers
{
    [Route("api/{orgName}/{projectName}/[controller]")]
    [Authorize]
    [ApiController]
    [AuthenticateAttribute]
    [ValidateModel]
    public class GraphController : ControllerBase
    {
        private readonly IProjectRepository projRepo;
        private readonly IMapper mapper;
        private readonly ITeamRepository teamRepo;
        public GraphController(IProjectRepository projRepo, IMapper mapper, ITeamRepository teamRepo)
        {
            this.teamRepo = teamRepo;
            this.mapper = mapper;
            this.projRepo = projRepo;
        }

        // [Route("CreateArrow")]
        // [HttpPost]
        // public async Task<IActionResult> CreateArrow(string orgName, string projectName, ArrowForCreationDTO2 arrForCreate)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var activitiesGraph = await this.projRepo.GetActivitiesGraphAsync(orgName, projectName);
        //         if (activitiesGraph != null)
        //         {
        //             var newArrow = this.mapper.Map<Arrow>(arrForCreate);

        //             if (activitiesGraph.AddArrow(newArrow))
        //             {
        //                 activitiesGraph.NormalizeGraph();
        //                 activitiesGraph.ComputeValues();
        //                 activitiesGraph.RemoveFakeNodes();
        //                 if (await this.projRepo.SaveChanges())
        //                 {
        //                     return Ok(newArrow);
        //                 }
        //             }
        //             return NotFound("can not found from activity or/and to activity");
        //         }
        //         return NotFound();
        //     }
        //     return BadRequest(ModelState);
        // }

        // [HttpPut]
        // public async Task<IActionResult> UpdateArrow(string orgName, string projectName, ArrowForCreationDTO2 arrForUpdate)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var activitiesGraph = await this.projRepo.GetActivitiesGraphAsync(orgName, projectName);
        //         if (activitiesGraph != null)
        //         {
        //             var newArrow = this.mapper.Map<Arrow>(arrForUpdate);
        //             var modifiedArrow = activitiesGraph.UpdateArrow(newArrow);
        //             if (modifiedArrow != null)
        //             {
        //                 activitiesGraph.NormalizeGraph();
        //                 activitiesGraph.InitializeValues();
        //                 activitiesGraph.ComputeValues();
        //                 activitiesGraph.RemoveFakeNodes();
        //                 this.projRepo.UpdateActivitiesGraph(activitiesGraph.GetActivitiesGraph());
        //                 this.projRepo.UpdateActivityOrArrow(modifiedArrow);

        //                 if (await this.projRepo.SaveChanges())
        //                 {
        //                     return Ok(newArrow);
        //                 }
        //             }
        //             return NotFound("can not found from activity or/and to activity");
        //         }
        //         return NotFound();
        //     }
        //     return BadRequest(ModelState);
        // }

        // [HttpPost]
        // // [EnableQuery()]
        // public async Task<IActionResult> CreateActivity(string orgName, string projectName, [FromBody]ActivityForCreationDTO actForCreate)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var ActivitiesGraph = await this.projRepo.GetActivitiesGraphAsync(orgName, projectName);
        //         if (ActivitiesGraph != null)
        //         {
        //             var newAct = this.mapper.Map<Activity>(actForCreate);
        //             newAct.NormalizedProjectName = projectName;
        //             if (ActivitiesGraph.AddNode(newAct))
        //             {
        //                 // ActivitiesGraph.InitializeValues();
        //                 // ActivitiesGraph.NormalizeGraph();
        //                 // ActivitiesGraph.ComputeValues();
        //                 // ActivitiesGraph.RemoveFakeNodes();
        //                 this.projRepo.UpdateActivitiesGraph(ActivitiesGraph.GetActivitiesGraph());
        //                 if (await this.projRepo.SaveChanges())
        //                 {
        //                     return Ok(ActivitiesGraph.GetGraph());
        //                 }
        //                 return BadRequest($"Failed to create {actForCreate.Name} Activity");
        //             }
        //             return BadRequest($"{actForCreate.Name} already exists in this project");
        //         }
        //         return NotFound();
        //     }
        //     return BadRequest(ModelState);
        // }

        // [HttpGet("{name}", Name = "GetActivity")]
        // // [EnableQuery()]
        // public async Task<IActionResult> GetActivity(string orgName, string projectName, string name)
        // {
        //     var activitiesGraph = await this.projRepo.GetActivitiesGraphAsync(orgName, projectName);
        //     if (activitiesGraph != null)
        //     {
        //         return Ok(activitiesGraph.GetNode(name));
        //     }
        //     return NotFound();
        // }

        // [HttpPut("{name}")]
        // public async Task<IActionResult> UpdateActivity(string orgName, string projectName, string name, [FromBody]ActivityForCreationDTO actForUpdate)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var ActivitiesGraph = await this.projRepo.GetActivitiesGraphAsync(orgName, projectName);
        //         if (ActivitiesGraph != null)
        //         {
        //             var newAct = this.mapper.Map<Activity>(actForUpdate);
        //             newAct.NormalizedProjectName = projectName;
        //             var modifiedNode = ActivitiesGraph.UpdateNode(name, newAct);
        //             if (modifiedNode != null)
        //             {
        //                 this.projRepo.UpdateActivityOrArrow(modifiedNode);
        //                 if (await this.projRepo.SaveChanges())
        //                 {
        //                     return Ok(ActivitiesGraph.GetGraph());
        //                 }
        //                 return BadRequest($"Failed to modified {actForUpdate.Name} Activity");
        //             }
        //             return BadRequest($"{actForUpdate.Name} not exists in this project");
        //         }
        //         return NotFound();
        //     }
        //     return BadRequest(ModelState);
        // }

        // [HttpDelete("{name}")]
        // public async Task<IActionResult> DeleteActivity(string orgName, string projectName, string name)
        // {
        //     var activitiesGraph = await this.projRepo.GetActivitiesGraphAsync(orgName, projectName);
        //     if (activitiesGraph != null)
        //     {
        //         var removedNode = activitiesGraph.RemoveNode(name);
        //         if (removedNode != null)
        //         {
        //             this.projRepo.RemoveActivity(removedNode);
        //             // activitiesGraph.InitializeValues();
        //             // activitiesGraph.ComputeValues();
        //             // activitiesGraph.RemoveFakeNodes();
        //             this.projRepo.UpdateActivitiesGraph(activitiesGraph.GetActivitiesGraph());
        //             if (await this.projRepo.SaveChanges())
        //             {
        //                 return Ok();
        //             }
        //         }
        //         return BadRequest($"{name} not exists in this project");
        //     }
        //     return NotFound();
        // }
        [HttpPost("{actvName}")]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.CONTRIBUTOR, ParamType = ParamType.PATH_PARAM)]

        public async Task<IActionResult> AssignToTeam(string orgName, string projectName, string actvName, [FromQuery] string teamName)
        {
            var project = HttpContext.Items["project"] as Project;
            var org = HttpContext.Items["organization"] as Organization;
            var member = HttpContext.Items["member"] as Member;
            var ActivitiesGraph = await this.projRepo.GetActivitiesGraphAsync(org, project);
            var actv = ActivitiesGraph.GetNode(actvName);
            var team = await this.teamRepo.FindByNameAsync(org, teamName);
            if (team != null)
            {
                this.projRepo.AssignActivityAsync(org, project, actv, team);
                if (await this.projRepo.SaveChangesAsync())
                {
                    return Ok(new { message = $"{teamName} team has been assigned successfully to @{actvName} activity" });
                }
                return BadRequest($"Failed to assign @{teamName} to @{actvName}");
            }
            return NotFound($"@{teamName} is not exist");
        }

        [HttpGet("{actvName}")]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.CONTRIBUTOR, ParamType = ParamType.PATH_PARAM)]

        public async Task<IActionResult> GetOne(string orgName, string projectName, string actvName)
        {
            var project = HttpContext.Items["project"] as Project;
            var org = HttpContext.Items["organization"] as Organization;
            var member = HttpContext.Items["member"] as Member;
            var user = HttpContext.Items["current-user"] as User;
            IAoNGraph ActivitiesGraph;
            Activity actv;
            ActivityToReturnDTO actvToReturn;
            if (member.Team == null && member.Type == @short.OWNER)
            {
                ActivitiesGraph = await this.projRepo.GetActivitiesGraphAsync(org, project);
                actv = ActivitiesGraph.GetNode(actvName);
                actvToReturn = this.mapper.Map<ActivityToReturnDTO>(actv);
                return Ok(actvToReturn);
            }
            else
            {
                if (member.Team != null)
                {
                    ActivitiesGraph = await this.projRepo.GetActivitiesGraphAsync(org, project);
                    actv = ActivitiesGraph.GetNode(actvName);
                    if (actv == null)
                    {
                        return NotFound($"@{actvName} activity not exist");
                    }
                    var rs = await this.projRepo.AllActivityResources(org, project, actv);
                    foreach (var item in rs)
                    {
                        if (item.NormalizedTeamName == member.Team.Name)
                        {
                            actvToReturn = this.mapper.Map<ActivityToReturnDTO>(actv);
                            return Ok(actvToReturn);
                        }
                    }
                }
            }
            return Unauthorized();
        }
        [HttpGet]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> GetActivitiesGraph(string orgName, string projectName)
        {
            var project = HttpContext.Items["project"] as Project;
            var org = HttpContext.Items["organization"] as Organization;
            var ActivitiesGraph = await this.projRepo.GetActivitiesGraphAsync(org, project);
            if (ActivitiesGraph != null)
            {
                var startNode = this.mapper.Map<ActivityToReturnDTO>(ActivitiesGraph.GetGraph());
                var graph = new GraphToReturnDTO { StartNode = startNode, EarlyStart = startNode.EarlyStart };
                return Ok(graph);
            }
            return NotFound();
        }
        [HttpPost]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> Create(string orgName, string projectName, [FromBody] GraphForCreationDTO graphForCreate)
        {

            var project = HttpContext.Items["project"] as Project;
            var org = HttpContext.Items["organization"] as Organization;
            var startNode = this.mapper.Map<Activity>(graphForCreate.StartNode);
            var graph = new AoNGraph(new List<Activity>());
            startNode.EarlyStart = graphForCreate.EarlyStart;
            graph.AddGraph(startNode);
            if (graph.AddGraph(startNode))
            {
                await this.projRepo.UpdateActivitiesGraphAsync(org, project, graph);
                if (await this.projRepo.SaveChangesAsync())
                {
                    var graphToReturn = this.mapper.Map<ActivityToReturnDTO>(graph.GetGraph());
                    return Ok(graphToReturn);
                }
            }
            return BadRequest($"Failed to create Activities graph");
        }
        [HttpDelete]
        [AuthAccessToOrganization(Role = @short.MEMBER, ParamType = ParamType.PATH_PARAM)]
        [AuthAccessToProject(Role = AccessType.MANAGER, ParamType = ParamType.PATH_PARAM)]
        public async Task<IActionResult> DeleteActivitiesGraph(string orgName, string projectName)
        {
            var project = HttpContext.Items["project"] as Project;
            var org = HttpContext.Items["organization"] as Organization;
            var graph = new AoNGraph(new List<Activity>());
            if (await this.projRepo.UpdateActivitiesGraphAsync(org, project, graph))
            {
                return Ok(new { message = $"graph of @{projectName} project has been deleted successfully" });
            }
            return BadRequest($"Failed to remove graph of @{projectName} project");
        }
    }
}