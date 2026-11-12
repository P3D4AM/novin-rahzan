using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public interface IAuthorizationPolicyProvider
{
	Task<AuthorizationPolicy> GetPolicyAsync(string policyName);

	Task<AuthorizationPolicy> GetDefaultPolicyAsync();
}
