using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtualDean.Enties;

namespace VirtualDean.Authorization
{
    public class ShouldBeCantor : IAuthorizationRequirement
    {
    }

    public class ShouldBeCantorHandler : AuthorizationHandler<ShouldBeCantor>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldBeCantor requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            var claims = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var role = claims.Value;
            if (role == BrotherStatus.CANTOR)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
