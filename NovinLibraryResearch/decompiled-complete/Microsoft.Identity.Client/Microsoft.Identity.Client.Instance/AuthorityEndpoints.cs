using System.Threading.Tasks;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.Instance;

internal class AuthorityEndpoints
{
	public string AuthorizationEndpoint { get; }

	public string TokenEndpoint { get; }

	public string SelfSignedJwtAudience { get; }

	public string DeviceCodeEndpoint { get; }

	public AuthorityEndpoints(string authorizationEndpoint, string tokenEndpoint, string deviceCodeEndpoint)
	{
		AuthorizationEndpoint = authorizationEndpoint;
		TokenEndpoint = tokenEndpoint;
		SelfSignedJwtAudience = tokenEndpoint;
		DeviceCodeEndpoint = deviceCodeEndpoint;
	}

	public static async Task UpdateAuthorityEndpointsAsync(AuthenticationRequestParameters requestParameters)
	{
		await UpdateAuthorityWithPreferredNetworkHostAsync(requestParameters).ConfigureAwait(continueOnCapturedContext: false);
		requestParameters.Endpoints = await requestParameters.RequestContext.ServiceBundle.AuthorityEndpointResolutionManager.ResolveEndpointsAsync(requestParameters.AuthorityInfo, requestParameters.LoginHint, requestParameters.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async Task UpdateAuthorityWithPreferredNetworkHostAsync(AuthenticationRequestParameters requestParameters)
	{
		InstanceDiscoveryMetadataEntry instanceDiscoveryMetadataEntry = await requestParameters.RequestContext.ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryAsync(requestParameters.AuthorityInfo.CanonicalAuthority, requestParameters.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
		requestParameters.Authority = Authority.CreateAuthorityWithEnvironment(requestParameters.AuthorityInfo, instanceDiscoveryMetadataEntry.PreferredNetwork);
	}
}
