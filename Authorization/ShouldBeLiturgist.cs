using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Authorization
{
    public class ShouldBeLiturgist : IAuthorizationRequirement
    {
    }

    public class ShouldBeLiturgistHandler : AuthorizationHandler<ShouldBeLiturgist>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldBeLiturgist requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            var claims = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var role = claims.Value;
            if (role == BrotherStatus.LITURGIST)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
