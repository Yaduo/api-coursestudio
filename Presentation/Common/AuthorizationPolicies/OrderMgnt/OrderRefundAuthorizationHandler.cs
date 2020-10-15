using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CourseStudio.Domain.TraversalModel.Identities;

namespace CourseStudio.Presentation.Common.AuthorizationPolicies
{
    public class OrderRefundAuthorizationHandler : AuthorizationHandler<OrderRefundRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderRefundRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scopes"))
            {
                return Task.CompletedTask;
            }

            var scopes = context.User.Claims.Where(c => c.Type == "scopes").Select(c => c.Value).ToList();
            if (!scopes.Contains(ApplicationPolicies.Claims.OrderMgnt.Refund))
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}