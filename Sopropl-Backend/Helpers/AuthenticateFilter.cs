using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sopropl_Backend.Data;
using Sopropl_Backend.Models;
using Sopropl_Backend.Repositories;

namespace Sopropl_Backend.Helpers
{
    // [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthenticateAttribute : ActionFilterAttribute//, IAsyncActionFilter
    {

        // public override void OnActionExecuting(ActionExecutingContext context)
        // {

        //     IUserRepository userRepo = (IUserRepository)context.HttpContext.RequestServices.GetService(typeof(IUserRepository));
        //     var userPrancipal = context.HttpContext.User;
        //     var user = userRepo.FindByName(userPrancipal.FindFirst(ClaimTypes.Name).Value);
        //     if (user == null || user.Id != userPrancipal.FindFirst(ClaimTypes.NameIdentifier).Value)
        //     {
        //         context.Result = new UnauthorizedResult();
        //     }
        //     else
        //     {
        //         context.HttpContext.Items.Add("current-user", user);
        //     }
        // }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IUserRepository userRepo = (IUserRepository)context.HttpContext.RequestServices.GetService(typeof(IUserRepository));
            var userPrancipal = context.HttpContext.User;
            var user = await userRepo.FindByNameAsync(userPrancipal.FindFirst(ClaimTypes.Name).Value);
            if (user == null || user.Id != userPrancipal.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                context.HttpContext.Items.Add("current-user", user);
                var resultContext = await next();
            }
        }
    }
}