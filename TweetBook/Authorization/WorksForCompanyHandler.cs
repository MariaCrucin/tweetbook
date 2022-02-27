using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TweetBook.Authorization
{
    public class WorksForCompanyHandler : AuthorizationHandler<WorksForCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksForCompanyRequirement requirement)
        {
            var userEmailAdress = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            if (userEmailAdress.EndsWith(requirement.DomainName))
            {
                context.Succeed(requirement);

            }
            else {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
