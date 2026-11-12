using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Identity.Client.Instance;

internal class AadAuthority : Authority
{
	public const string DefaultTrustedHost = "login.microsoftonline.com";

	public const string AADCanonicalAuthorityTemplate = "https://{0}/{1}/";

	private const string TokenEndpointTemplate = "{0}oauth2/v2.0/token";

	private const string DeviceCodeEndpointTemplate = "{0}oauth2/v2.0/devicecode";

	private const string AuthorizationEndpointTemplate = "{0}oauth2/v2.0/authorize";

	private static readonly ISet<string> s_tenantlessTenantNames = new HashSet<string>(new string[3] { "common", "organizations", "consumers" }, StringComparer.OrdinalIgnoreCase);

	internal override string TenantId { get; }

	internal AadAuthority(AuthorityInfo authorityInfo)
		: base(authorityInfo)
	{
		TenantId = Authority.GetFirstPathSegment(base.AuthorityInfo.CanonicalAuthority);
	}

	internal bool IsCommonOrganizationsOrConsumersTenant()
	{
		return IsCommonOrganizationsOrConsumersTenant(TenantId);
	}

	internal static bool IsCommonOrganizationsOrConsumersTenant(string tenantId)
	{
		if (!string.IsNullOrEmpty(tenantId))
		{
			return s_tenantlessTenantNames.Contains(tenantId);
		}
		return false;
	}

	internal override string GetTenantedAuthority(string tenantId)
	{
		if (!string.IsNullOrEmpty(tenantId) && IsCommonOrganizationsOrConsumersTenant())
		{
			Uri uri = new Uri(base.AuthorityInfo.CanonicalAuthority);
			return string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/", uri.Authority, tenantId);
		}
		return base.AuthorityInfo.CanonicalAuthority;
	}

	internal override AuthorityEndpoints GetHardcodedEndpoints()
	{
		string tokenEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}oauth2/v2.0/token", base.AuthorityInfo.CanonicalAuthority);
		string authorizationEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}oauth2/v2.0/authorize", base.AuthorityInfo.CanonicalAuthority);
		string deviceCodeEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}oauth2/v2.0/devicecode", base.AuthorityInfo.CanonicalAuthority);
		return new AuthorityEndpoints(authorizationEndpoint, tokenEndpoint, deviceCodeEndpoint);
	}
}
