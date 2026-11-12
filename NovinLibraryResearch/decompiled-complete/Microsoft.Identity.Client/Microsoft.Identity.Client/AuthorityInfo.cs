using System;
using System.Globalization;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client;

internal class AuthorityInfo
{
	public string Host { get; }

	public string CanonicalAuthority { get; }

	public AuthorityType AuthorityType { get; }

	public string UserRealmUriPrefix { get; }

	public bool ValidateAuthority { get; }

	public bool AutoDetectRegion { get; set; }

	internal bool IsDefaultAuthority => string.Equals(CanonicalAuthority, "https://login.microsoftonline.com/common/", StringComparison.OrdinalIgnoreCase);

	public AuthorityInfo(AuthorityType authorityType, string authority, bool validateAuthority)
	{
		AuthorityType = authorityType;
		ValidateAuthority = validateAuthority;
		UriBuilder uriBuilder = new UriBuilder(authority);
		Host = uriBuilder.Host;
		UserRealmUriPrefix = UriBuilderExtensions.GetHttpsUriWithOptionalPort(string.Format(CultureInfo.InvariantCulture, "https://{0}/common/userrealm/", Host), uriBuilder.Port);
		if (AuthorityType == AuthorityType.B2C)
		{
			Uri uri = new Uri(authority);
			string[] array = uri.AbsolutePath.Substring(1).Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 3)
			{
				throw new ArgumentException("B2C 'authority' Uri should have at least 3 segments in the path (i.e. https://<host>/tfp/<tenant>/<policy>/...)");
			}
			CanonicalAuthority = string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/{2}/{3}/", uri.Authority, array[0], array[1], array[2]);
		}
		else
		{
			CanonicalAuthority = UriBuilderExtensions.GetHttpsUriWithOptionalPort(string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/", uriBuilder.Uri.Authority, GetFirstPathSegment(authority)), uriBuilder.Port);
		}
	}

	private AuthorityInfo(string host, string canonicalAuthority, AuthorityType authorityType, string userRealmUriPrefix, bool validateAuthority, bool autoDetectRegion)
	{
		Host = host;
		CanonicalAuthority = canonicalAuthority;
		AuthorityType = authorityType;
		UserRealmUriPrefix = userRealmUriPrefix;
		ValidateAuthority = validateAuthority;
		AutoDetectRegion = autoDetectRegion;
	}

	public AuthorityInfo(AuthorityInfo other)
		: this(other.Host, other.CanonicalAuthority, other.AuthorityType, other.UserRealmUriPrefix, other.ValidateAuthority, other.AutoDetectRegion)
	{
	}

	internal static AuthorityInfo FromAuthorityUri(string authorityUri, bool validateAuthority)
	{
		string authority = CanonicalizeAuthorityUri(authorityUri);
		ValidateAuthorityUri(authority);
		AuthorityType authorityType = Authority.GetAuthorityType(authority);
		if (authorityType == AuthorityType.B2C)
		{
			validateAuthority = false;
		}
		return new AuthorityInfo(authorityType, authority, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(Uri cloudInstanceUri, Guid tenantId, bool validateAuthority)
	{
		return FromAuthorityUri(new Uri(cloudInstanceUri, tenantId.ToString("D")).AbsoluteUri, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(Uri cloudInstanceUri, string tenant, bool validateAuthority)
	{
		if (Guid.TryParse(tenant, out var result))
		{
			return FromAadAuthority(cloudInstanceUri, result, validateAuthority);
		}
		return FromAuthorityUri(new Uri(cloudInstanceUri, tenant).AbsoluteUri, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(AzureCloudInstance azureCloudInstance, Guid tenantId, bool validateAuthority)
	{
		string authorityUri = GetAuthorityUri(azureCloudInstance, AadAuthorityAudience.AzureAdMyOrg, tenantId.ToString("D"));
		return new AuthorityInfo(AuthorityType.Aad, authorityUri, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(AzureCloudInstance azureCloudInstance, string tenant, bool validateAuthority)
	{
		if (Guid.TryParse(tenant, out var result))
		{
			return FromAadAuthority(azureCloudInstance, result, validateAuthority);
		}
		string authorityUri = GetAuthorityUri(azureCloudInstance, AadAuthorityAudience.AzureAdMyOrg, tenant);
		return new AuthorityInfo(AuthorityType.Aad, authorityUri, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(AzureCloudInstance azureCloudInstance, AadAuthorityAudience authorityAudience, bool validateAuthority)
	{
		string authorityUri = GetAuthorityUri(azureCloudInstance, authorityAudience);
		return new AuthorityInfo(AuthorityType.Aad, authorityUri, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(AadAuthorityAudience authorityAudience, bool validateAuthority)
	{
		string authorityUri = GetAuthorityUri(AzureCloudInstance.AzurePublic, authorityAudience);
		return new AuthorityInfo(AuthorityType.Aad, authorityUri, validateAuthority);
	}

	internal static AuthorityInfo FromAadAuthority(string authorityUri, bool validateAuthority)
	{
		return new AuthorityInfo(AuthorityType.Aad, authorityUri, validateAuthority);
	}

	internal static AuthorityInfo FromAdfsAuthority(string authorityUri, bool validateAuthority)
	{
		return new AuthorityInfo(AuthorityType.Adfs, authorityUri, validateAuthority);
	}

	internal static AuthorityInfo FromB2CAuthority(string authorityUri)
	{
		return new AuthorityInfo(AuthorityType.B2C, authorityUri, validateAuthority: false);
	}

	internal static string GetCloudUrl(AzureCloudInstance azureCloudInstance)
	{
		return azureCloudInstance switch
		{
			AzureCloudInstance.AzurePublic => "https://login.microsoftonline.com", 
			AzureCloudInstance.AzureChina => "https://login.chinacloudapi.cn", 
			AzureCloudInstance.AzureGermany => "https://login.microsoftonline.de", 
			AzureCloudInstance.AzureUsGovernment => "https://login.microsoftonline.us", 
			_ => throw new ArgumentException("azureCloudInstance"), 
		};
	}

	internal static string GetAadAuthorityAudienceValue(AadAuthorityAudience authorityAudience, string tenantId)
	{
		switch (authorityAudience)
		{
		case AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount:
			return "common";
		case AadAuthorityAudience.AzureAdMultipleOrgs:
			return "organizations";
		case AadAuthorityAudience.PersonalMicrosoftAccount:
			return "consumers";
		case AadAuthorityAudience.AzureAdMyOrg:
			if (string.IsNullOrWhiteSpace(tenantId))
			{
				throw new InvalidOperationException("When specifying AadAuthorityAudience.AzureAdMyOrg, you must also specify a tenant domain or tenant guid.");
			}
			return tenantId;
		default:
			throw new ArgumentException("authorityAudience");
		}
	}

	internal static string CanonicalizeAuthorityUri(string uri)
	{
		if (!string.IsNullOrWhiteSpace(uri) && !uri.EndsWith("/", StringComparison.OrdinalIgnoreCase))
		{
			uri += "/";
		}
		return uri.ToLowerInvariant();
	}

	private static void ValidateAuthorityUri(string authority)
	{
		if (string.IsNullOrWhiteSpace(authority))
		{
			throw new ArgumentNullException("authority");
		}
		if (!Uri.IsWellFormedUriString(authority, UriKind.Absolute))
		{
			throw new ArgumentException("'authority' should be in Uri format", "authority");
		}
		Uri uri = new Uri(authority);
		if (uri.Scheme != "https")
		{
			throw new ArgumentException("'authority' should use the 'https' scheme", "authority");
		}
		if (string.IsNullOrWhiteSpace(uri.AbsolutePath.Substring(1)))
		{
			throw new ArgumentException("'authority' Uri should have at least one segment in the path (i.e. https://<host>/<path>/...)", "authority");
		}
		string[] array = uri.AbsolutePath.Substring(1).Split(new char[1] { '/' });
		if (array == null || array.Length == 0)
		{
			throw new ArgumentException("'authority' Uri should have at least one segment in the path (i.e. https://<host>/<path>/...)");
		}
	}

	private static string GetAuthorityUri(AzureCloudInstance azureCloudInstance, AadAuthorityAudience authorityAudience, string tenantId = null)
	{
		string cloudUrl = GetCloudUrl(azureCloudInstance);
		string aadAuthorityAudienceValue = GetAadAuthorityAudienceValue(authorityAudience, tenantId);
		return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", cloudUrl, aadAuthorityAudienceValue);
	}

	private static string GetFirstPathSegment(string authority)
	{
		if (new Uri(authority).Segments.Length >= 2)
		{
			return new Uri(authority).Segments[1].TrimEnd(new char[1] { '/' });
		}
		throw new InvalidOperationException("Authority should be in the form <host>/<audience>, for example https://login.microsoftonline.com/common");
	}
}
