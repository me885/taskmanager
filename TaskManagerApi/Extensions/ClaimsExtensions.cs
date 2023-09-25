using System.Security.Claims;

namespace TaskManagerApi.Extensions;

static class ClaimsExtensions
{
    public static Guid GetUserIdFromClaims(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.Claims.Single(x => x.Type == "Id").Value);
    }
}