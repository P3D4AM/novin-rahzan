using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance;

internal interface IAuthorityEndpointResolutionManager
{
	Task<AuthorityEndpoints> ResolveEndpointsAsync(AuthorityInfo authorityInfo, string userPrincipalName, RequestContext requestContext);
}
