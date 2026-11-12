using System;

namespace Microsoft.Identity.Client.Instance;

internal class B2CAuthority : AadAuthority
{
	public const string Prefix = "tfp";

	public const string B2CCanonicalAuthorityTemplate = "https://{0}/{1}/{2}/{3}/";

	internal override string TenantId { get; }

	internal B2CAuthority(AuthorityInfo authorityInfo)
		: base(authorityInfo)
	{
		TenantId = new Uri(base.AuthorityInfo.CanonicalAuthority).Segments[2].TrimEnd(new char[1] { '/' });
	}

	internal override string GetTenantedAuthority(string tenantId)
	{
		return base.AuthorityInfo.CanonicalAuthority;
	}
}
