using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Helpers
{
    public class AuthAccessToProject : ActionFilterAttribute
    {
        public short Role { get; set; }
        public ParamType ParamType { get; set; }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.Items["current-user"] as User;
            var org = context.HttpContext.Items["organization"] as Organization;
            var member = context.HttpContext.Items["member"] as Member;
            string projectName;

            if (this.ParamType == ParamType.PATH_PARAM)
            {
                projectName = context.HttpContext.GetRouteValue("projectName") as string;
            }
            else
            {
                projectName = context.HttpContext.Request.Query["projectName"].ToString();
            }
            if (user != null && org != null && member != null)
            {
                if (projectName != null)
                {
                    IProjectRepository projectRepo = (IProjectRepository)context.HttpContext.RequestServices.GetService(typeof(IProjectRepository));
                    var project = await projectRepo.FindByNameAsync(org, projectName);
                    if (project != null)
                    {
                        if (member.Type == @short.OWNER)
                        {
                            context.HttpContext.Items.Add("project", project);
                            var resultContext = await next();
                        }
                        else if (member.Team != null)
                        {
                            var access = await projectRepo.FindAccessAsync(org, project, member.Team);
                            if (access.Permission >= Role)
                            {
                                context.HttpContext.Items.Add("project", project);
                                var resultContext = await next();
                            }
                            else
                            {
                                context.Result = new UnauthorizedResult();
                            }
                        }
                        else
                        {
                            context.Result = new UnauthorizedResult();
                        }
                    }
                    else
                    {
                        context.Result = new NotFoundResult();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}