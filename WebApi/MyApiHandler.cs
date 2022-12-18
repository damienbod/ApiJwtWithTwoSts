using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApi;

public class MyApiHandler : AuthorizationHandler<MyApiRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyApiRequirement requirement)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (requirement == null)
            throw new ArgumentNullException(nameof(requirement));

        var client_id = context.User.Claims.FirstOrDefault(t => t.Type == "client_id");
        var scope = context.User.Claims.FirstOrDefault(t => t.Type == "scope");

        if (AccessTokenValid(client_id, scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private bool AccessTokenValid(Claim? client_id, Claim? scope)
    {
        if (client_id != null && client_id.Value == "CC_STS_A")
        {
            return StsAScopeAValid(scope);
        }

        if (client_id != null && client_id.Value == "CC_STS_B")
        {
            return StsBScopeBValid(scope);
        }

        return false;
    }
    private static bool StsAScopeAValid(Claim? scope)
    {
        if (scope != null && scope.Value == "scope_a")
        {
            return true;
        }

        return false;
    }

    private static bool StsBScopeBValid(Claim? scope)
    {
        if (scope != null && scope.Value == "scope_b")
        {
            return true;
        }

        return false;
    }

}
