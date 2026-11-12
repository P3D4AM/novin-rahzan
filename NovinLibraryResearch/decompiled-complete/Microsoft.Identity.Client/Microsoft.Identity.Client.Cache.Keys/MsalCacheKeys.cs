using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Identity.Client.Cache.Keys;

internal class MsalCacheKeys
{
	internal enum iOSCredentialAttrType
	{
		AccessToken = 2001,
		RefreshToken = 2002,
		IdToken = 2003,
		Password = 2004,
		AppMetadata = 3001
	}

	public const string CacheKeyDelimiter = "-";

	internal static readonly Dictionary<string, int> iOSAuthorityTypeToAttrType = new Dictionary<string, int>
	{
		{
			CacheAuthorityType.AAD.ToString(),
			1001
		},
		{
			CacheAuthorityType.MSA.ToString(),
			1002
		},
		{
			CacheAuthorityType.MSSTS.ToString(),
			1003
		},
		{
			CacheAuthorityType.OTHER.ToString(),
			1004
		}
	};

	public static string GetCredentialKey(string homeAccountId, string environment, string keyDescriptor, string clientId, string tenantId, string scopes, params string[] additionalKeys)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(homeAccountId ?? "");
		stringBuilder.Append("-");
		stringBuilder.Append(environment);
		stringBuilder.Append("-");
		stringBuilder.Append(keyDescriptor);
		stringBuilder.Append("-");
		stringBuilder.Append(clientId);
		stringBuilder.Append("-");
		stringBuilder.Append(tenantId ?? "");
		stringBuilder.Append("-");
		stringBuilder.Append(scopes ?? "");
		foreach (string item in additionalKeys ?? Enumerable.Empty<string>())
		{
			stringBuilder.Append("-");
			stringBuilder.Append(item);
		}
		return stringBuilder.ToString().ToLowerInvariant();
	}

	public static string GetiOSAccountKey(string homeAccountId, string environment)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(homeAccountId ?? "");
		stringBuilder.Append("-");
		stringBuilder.Append(environment);
		return stringBuilder.ToString().ToLowerInvariant();
	}

	public static string GetiOSServiceKey(string keyDescriptor, string clientId, string tenantId, string scopes, params string[] extraKeyParts)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(keyDescriptor);
		stringBuilder.Append("-");
		stringBuilder.Append(clientId);
		stringBuilder.Append("-");
		stringBuilder.Append(tenantId ?? "");
		stringBuilder.Append("-");
		stringBuilder.Append(scopes ?? "");
		foreach (string item in extraKeyParts ?? Enumerable.Empty<string>())
		{
			stringBuilder.Append("-");
			stringBuilder.Append(item);
		}
		return stringBuilder.ToString().ToLowerInvariant();
	}

	public static string GetiOSGenericKey(string keyDescriptor, string clientId, string tenantId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(keyDescriptor);
		stringBuilder.Append("-");
		stringBuilder.Append(clientId);
		stringBuilder.Append("-");
		stringBuilder.Append(tenantId ?? "");
		return stringBuilder.ToString().ToLowerInvariant();
	}
}
