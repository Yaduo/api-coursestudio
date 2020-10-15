using System;
using System.Text;
using System.Threading.Tasks;
using CourseStudio.Application.Common.Identities;
using Microsoft.AspNetCore.Authorization;

namespace CourseStudio.Presentation.Common.AuthorizationPolicies
{
    public class AccessTokenVerificationAuthorizationHandler : AuthorizationHandler<AccessTokenVerificationRequirement>
    {
        private readonly IIdentityService _identityService;

        public AccessTokenVerificationAuthorizationHandler(
            IIdentityService identityService
        )
        {
            _identityService = identityService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessTokenVerificationRequirement requirement)
        {
            var mvcContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            if (mvcContext == null)
            {
                context.Fail();
            }

            var authorizationHeader = mvcContext.HttpContext.Request.Headers["Authorization"];
            if (authorizationHeader.Count < 1)
            {
                context.Fail();
            }

            var token = authorizationHeader[0].Split(' ')[1];
            if (!(await _identityService.IsTokenAuthorizedAsync(token)))
            {
                var Response = mvcContext.HttpContext.Response;
                var message = Encoding.UTF8.GetBytes("Access Token Blocked ");
                Response.OnStarting(async () =>
                {
                    Response.StatusCode = 401;
                    await Response.Body.WriteAsync(message, 0, message.Length);
                });

                // Requirement Fail, return 401
                context.Fail();
            }

            context.Succeed(requirement);
        }
    }
}
