using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ioc.Data.CommonMethod
{
    public class GetUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserName()
        {
            ClaimsPrincipal claimsPrincipal = _httpContextAccessor.HttpContext?.User;

            if (claimsPrincipal?.Identity?.IsAuthenticated == true)
            {
                Claim userNameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
                if (userNameClaim != null)
                {
                    return userNameClaim.Value;
                }
            }

            return null;
        }
    }
}
