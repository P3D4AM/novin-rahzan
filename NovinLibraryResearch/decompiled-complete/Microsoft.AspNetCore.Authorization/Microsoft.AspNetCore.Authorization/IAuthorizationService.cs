using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public interface IAuthorizationService
{
	Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements);

	Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName);
}
