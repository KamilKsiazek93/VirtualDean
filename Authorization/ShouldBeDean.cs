using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Authorization
{
    internal class ShouldBeDean : IAuthorizationRequirement
    {
        
    }

    internal class ShouldBeDeanHandler : AuthorizationHandler<ShouldBeDean>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldBeDean requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            var claims = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var role = claims.Value;
            if (role == BrotherStatus.DEAN)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}