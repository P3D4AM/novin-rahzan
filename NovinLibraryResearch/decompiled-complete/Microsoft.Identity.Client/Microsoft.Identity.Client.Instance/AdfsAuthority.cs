using System.Globalization;

namespace Microsoft.Identity.Client.Instance;

internal class AdfsAuthority : Authority
{
	private const string TokenEndpointTemplate = "{0}oauth2/token";

	private const string AuthorizationEndpointTemplate = "{0}oauth2/authorize";

	private const string DeviceCodeEndpointTemplate = "{0}oauth2/devicecode";

	internal override string TenantId => null;

	public AdfsAuthority(AuthorityInfo authorityInfo)
		: base(authorityInfo)
	{
	}

	internal override string GetTenantedAuthority(string tenantId)
	{
		return base.AuthorityInfo.CanonicalAuthority;
	}

	internal override AuthorityEndpoints GetHardcodedEndpoints()
	{
		string tokenEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}oauth2/token", base.AuthorityInfo.CanonicalAuthority);
		string deviceCodeEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}oauth2/devicecode", base.AuthorityInfo.CanonicalAuthority);
		return new AuthorityEndpoints(string.Format(CultureInfo.InvariantCulture, "{0}oauth2/authorize", base.AuthorityInfo.CanonicalAuthority), tokenEndpoint, deviceCodeEndpoint);
	}
}
