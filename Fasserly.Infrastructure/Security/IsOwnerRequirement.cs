using Fasserly.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Security
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
    }

    public class IsOwnerRequirementHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseContext _context;

        public IsOwnerRequirementHandler(IHttpContextAccessor httpContextAccessor, DatabaseContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        //Authorize Owner's courses
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var currentUserName = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var trainingId = Guid.Parse(authContext.RouteData.Values["id"].ToString());
                var training = _context.Trainings.FindAsync(trainingId).Result;
                var owner = training.UserTrainings.FirstOrDefault(x => x.IsOwner);
                if (owner.UserFasserly.UserName == currentUserName)
                    context.Succeed(requirement);
            }
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
