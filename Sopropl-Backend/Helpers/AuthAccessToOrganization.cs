using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Helpers
{
    public enum ParamType
    {
        PATH_PARAM, QUERY_PARAM
    }
    public class AuthAccessToOrganization : ActionFilterAttribute
    {
        public short Role { get; set; }
        public ParamType ParamType { get; set; }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Items.ContainsKey("current-user"))
            {
                var user = (User)context.HttpContext.Items["current-user"];
                IOrganizationRepository organizationRepo = (IOrganizationRepository)context.HttpContext.RequestServices.GetService(typeof(IOrganizationRepository));
                string organizationName;
                if (this.ParamType == ParamType.PATH_PARAM)
                {
                    organizationName = context.HttpContext.GetRouteValue("orgName") as string;
                }
                else
                {
                    organizationName = context.HttpContext.Request.Query["organizationName"].ToString();
                }
                if (organizationName != null)
                {
                    var organization = await organizationRepo.FindByNameAsync(organizationName);
                    if (organization != null)
                    {
                        var member = await organizationRepo.FindMemberAsync(organization, user);
                        if (member != null)
                        {
                            if (member.Type >= this.Role)
                            {
                                context.HttpContext.Items.Add("member", member);
                                context.HttpContext.Items.Add("organization", organization);
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