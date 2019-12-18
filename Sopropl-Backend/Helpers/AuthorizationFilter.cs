using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sopropl_Backend.Data;

namespace Sopropl_Backend.Helpers
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var r = this.Roles;
            var p = this.Policy;
            SoproplDbContext dbocntext = (SoproplDbContext)context.HttpContext.RequestServices.GetService(typeof(SoproplDbContext));
            Console.WriteLine();
            Console.WriteLine();
            var v = context.HttpContext.User.FindFirst(ClaimTypes.Name);
            var user = dbocntext.Users.Find(v);
            Console.WriteLine(user.UserName);
            Console.WriteLine();
            Console.WriteLine();
            context.Result = new UnauthorizedResult();
        }


    }
}