using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Modules.Common.Authorization
{
    public record CanModifyRequirement : IAuthorizationRequirement;
    
    public abstract class CanModifyRequirmentHandler<TReq, TDbContext> : AuthorizationHandler<TReq, Guid> where TDbContext : DbContext where TReq : CanModifyRequirement
    {
        protected TDbContext m_dbContext;
        protected IHttpContextAccessor m_httpContextAccessor;

        protected CanModifyRequirmentHandler(TDbContext dbContext, IHttpContextAccessor accessor)
        {
            m_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            m_httpContextAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TReq requirement, Guid resource)
        {
            if (context.User.IsInRole("Handlers"))
            {
                context.Succeed(requirement);
                return;
            }
            var id = context.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

            if (await SpecialRequirmentFullied(Guid.Parse(id), resource))
                context.Succeed(requirement);
        }

        protected abstract Task<bool> SpecialRequirmentFullied(Guid userId, Guid id);  
    }
}
