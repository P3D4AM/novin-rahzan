using System;
using System.Diagnostics;

namespace Microsoft.Identity.Client.Instance;

[DebuggerDisplay("{AuthorityInfo.CanonicalAuthority}")]
internal abstract class Authority
{
	public AuthorityInfo AuthorityInfo { get; }

	internal abstract string TenantId { get; }

	protected Authority(AuthorityInfo authorityInfo)
	{
		if (authorityInfo == null)
		{
			throw new ArgumentNullException("authorityInfo");
		}
		AuthorityInfo = new AuthorityInfo(authorityInfo);
	}

	public static Authority CreateAuthorityForRequest(AuthorityInfo configAuthorityInfo, AuthorityInfo requestAuthorityInfo, string requestHomeAccountTenantId = null)
	{
		if (configAuthorityInfo == null)
		{
			throw new ArgumentNullException("configAuthorityInfo");
		}
		ValidateTypeMismatch(configAuthorityInfo, requestAuthorityInfo);
		switch (configAuthorityInfo.AuthorityType)
		{
		case AuthorityType.Adfs:
			if (requestAuthorityInfo != null)
			{
				return new AdfsAuthority(requestAuthorityInfo);
			}
			return new AdfsAuthority(configAuthorityInfo);
		case AuthorityType.B2C:
			if (requestAuthorityInfo != null)
			{
				CheckB2CAuthorityHost(requestAuthorityInfo, configAuthorityInfo);
				return new B2CAuthority(requestAuthorityInfo);
			}
			return new B2CAuthority(configAuthorityInfo);
		case AuthorityType.Aad:
		{
			if (requestAuthorityInfo == null)
			{
				return CreateAuthorityWithTenant(configAuthorityInfo, requestHomeAccountTenantId);
			}
			if (configAuthorityInfo.IsDefaultAuthority && requestAuthorityInfo.AuthorityType != 0)
			{
				return CreateAuthority(requestAuthorityInfo);
			}
			AadAuthority aadAuthority = new AadAuthority(requestAuthorityInfo);
			if (!aadAuthority.IsCommonOrganizationsOrConsumersTenant())
			{
				return aadAuthority;
			}
			return CreateAuthorityWithTenant(configAuthorityInfo, requestHomeAccountTenantId);
		}
		default:
			throw new MsalClientException("invalid_authority_type", "Unsupported authority type");
		}
	}

	private static void ValidateTypeMismatch(AuthorityInfo configAuthorityInfo, AuthorityInfo requestAuthorityInfo)
	{
		if (!configAuthorityInfo.IsDefaultAuthority && requestAuthorityInfo != null && configAuthorityInfo.AuthorityType != requestAuthorityInfo.AuthorityType)
		{
			throw new MsalClientException("authority_type_mismatch", MsalErrorMessage.AuthorityTypeMismatch(configAuthorityInfo.AuthorityType, requestAuthorityInfo.AuthorityType));
		}
	}

	public static Authority CreateAuthority(string authority, bool validateAuthority = false)
	{
		return CreateAuthority(AuthorityInfo.FromAuthorityUri(authority, validateAuthority));
	}

	public static Authority CreateAuthority(AuthorityInfo authorityInfo)
	{
		return authorityInfo.AuthorityType switch
		{
			AuthorityType.Adfs => new AdfsAuthority(authorityInfo), 
			AuthorityType.B2C => new B2CAuthority(authorityInfo), 
			AuthorityType.Aad => new AadAuthority(authorityInfo), 
			_ => throw new MsalClientException("invalid_authority_type", "Unsupported authority type " + authorityInfo.AuthorityType), 
		};
	}

	internal static Authority CreateAuthorityWithTenant(AuthorityInfo authorityInfo, string tenantId)
	{
		Authority authority = CreateAuthority(authorityInfo);
		if (string.IsNullOrEmpty(tenantId))
		{
			return authority;
		}
		return CreateAuthority(authority.GetTenantedAuthority(tenantId), authorityInfo.ValidateAuthority);
	}

	internal static Authority CreateAuthorityWithEnvironment(AuthorityInfo authorityInfo, string environment)
	{
		return CreateAuthority(new UriBuilder(authorityInfo.CanonicalAuthority)
		{
			Host = environment
		}.Uri.AbsoluteUri, authorityInfo.ValidateAuthority);
	}

	internal static string GetFirstPathSegment(string authority)
	{
		Uri uri = new Uri(authority);
		if (uri.Segments.Length > 1)
		{
			return uri.Segments[1].TrimEnd(new char[1] { '/' });
		}
		return string.Empty;
	}

	internal static AuthorityType GetAuthorityType(string authority)
	{
		string firstPathSegment = GetFirstPathSegment(authority);
		if (string.Equals(firstPathSegment, "adfs", StringComparison.OrdinalIgnoreCase))
		{
			return AuthorityType.Adfs;
		}
		if (string.Equals(firstPathSegment, "tfp", StringComparison.OrdinalIgnoreCase))
		{
			return AuthorityType.B2C;
		}
		return AuthorityType.Aad;
	}

	internal abstract string GetTenantedAuthority(string tenantId);

	internal abstract AuthorityEndpoints GetHardcodedEndpoints();

	private static void CheckB2CAuthorityHost(AuthorityInfo requestAuthorityInfo, AuthorityInfo configAuthorityInfo)
	{
		if (configAuthorityInfo.Host != requestAuthorityInfo.Host)
		{
			throw new MsalClientException("B2C_authority_host_mismatch", "The B2C authority host that was used when creating the client application is not the same authority host used in the AcquireToken call. See https://aka.ms/msal-net-b2c for details. ");
		}
	}

	internal static string GetEnviroment(string authority)
	{
		return new Uri(authority).Host;
	}
}
