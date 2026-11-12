using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authorization.Policy;

public interface IPolicyEvaluator
{
	Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context);

	Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource);
}
