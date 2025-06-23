using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;



namespace Infrastructure.Security
{
    public class RoleClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;
        if (identity == null || !identity.IsAuthenticated)
            return Task.FromResult(principal);

        var roleClaims = identity.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        foreach (var role in roleClaims)
        {
            if (!identity.HasClaim(ClaimTypes.Role, role.Value))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Value));
            }
        }

        return Task.FromResult(principal);
    }
}
}