using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal class MsalAccessTokenCacheItem : MsalCredentialCacheItemBase
{
	private string _tenantId;

	private string _scopes;

	internal string TenantId
	{
		get
		{
			return _tenantId;
		}
		set
		{
			_tenantId = value ?? string.Empty;
		}
	}

	internal string CachedAt { get; set; }

	internal string ExpiresOnUnixTimestamp { get; set; }

	internal string ExtendedExpiresOnUnixTimestamp { get; set; }

	internal string UserAssertionHash { get; set; }

	internal string KeyId { get; set; }

	internal string TokenType { get; set; }

	internal string RefreshOnUnixTimestamp { get; set; }

	internal bool IsAdfs { get; set; }

	internal string Authority
	{
		get
		{
			if (!IsAdfs)
			{
				return string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/", base.Environment, TenantId ?? "common");
			}
			return string.Format(CultureInfo.InvariantCulture, "https://{0}/{1}/", base.Environment, "adfs");
		}
	}

	internal HashSet<string> ScopeSet { get; }

	internal DateTimeOffset ExpiresOn => CoreHelpers.UnixTimestampStringToDateTime(ExpiresOnUnixTimestamp);

	internal DateTimeOffset ExtendedExpiresOn => CoreHelpers.UnixTimestampStringToDateTime(ExtendedExpiresOnUnixTimestamp);

	internal DateTimeOffset? RefreshOn
	{
		get
		{
			if (string.IsNullOrEmpty(RefreshOnUnixTimestamp))
			{
				return null;
			}
			return CoreHelpers.UnixTimestampStringToDateTime(RefreshOnUnixTimestamp);
		}
	}

	internal DateTimeOffset CachedAtOffset => CoreHelpers.UnixTimestampStringToDateTime(CachedAt);

	public bool IsExtendedLifeTimeToken { get; set; }

	internal MsalAccessTokenCacheItem(string preferredCacheEnv, string clientId, MsalTokenResponse response, string tenantId, string homeAccountId, string keyId = null)
		: this(preferredCacheEnv, clientId, response.Scope, tenantId, response.AccessToken, response.AccessTokenExpiresOn, response.AccessTokenExtendedExpiresOn, response.ClientInfo, homeAccountId, keyId, response.AccessTokenRefreshOn, response.TokenType)
	{
	}

	internal MsalAccessTokenCacheItem(string scopes)
	{
		base.CredentialType = "AccessToken";
		_scopes = scopes;
		ScopeSet = ScopeHelper.ConvertStringToScopeSet(_scopes);
	}

	internal MsalAccessTokenCacheItem(string preferredCacheEnv, string clientId, string scopes, string tenantId, string secret, DateTimeOffset accessTokenExpiresOn, DateTimeOffset accessTokenExtendedExpiresOn, string rawClientInfo, string homeAccountId, string keyId = null, DateTimeOffset? accessTokenRefreshOn = null, string tokenType = "Bearer")
		: this(scopes)
	{
		base.Environment = preferredCacheEnv;
		base.ClientId = clientId;
		TenantId = tenantId;
		base.Secret = secret;
		ExpiresOnUnixTimestamp = CoreHelpers.DateTimeToUnixTimestamp(accessTokenExpiresOn);
		ExtendedExpiresOnUnixTimestamp = CoreHelpers.DateTimeToUnixTimestamp(accessTokenExtendedExpiresOn);
		CachedAt = CoreHelpers.CurrDateTimeInUnixTimestamp().ToString(CultureInfo.InvariantCulture);
		base.RawClientInfo = rawClientInfo;
		KeyId = keyId;
		TokenType = tokenType;
		if (accessTokenRefreshOn.HasValue)
		{
			RefreshOnUnixTimestamp = CoreHelpers.DateTimeToUnixTimestamp(accessTokenRefreshOn.Value);
		}
		base.HomeAccountId = homeAccountId;
	}

	internal static MsalAccessTokenCacheItem FromJsonString(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return null;
		}
		return FromJObject(JObject.Parse(json));
	}

	internal static MsalAccessTokenCacheItem FromJObject(JObject j)
	{
		long num = JsonUtils.ExtractParsedIntOrZero(j, "cached_at");
		long num2 = JsonUtils.ExtractParsedIntOrZero(j, "expires_on");
		long num3 = JsonUtils.ExtractParsedIntOrZero(j, "ext_expires_on");
		long num4 = JsonUtils.ExtractParsedIntOrZero(j, "extended_expires_on");
		if (num4 == 0L && num3 > 0)
		{
			num4 = num3;
		}
		MsalAccessTokenCacheItem msalAccessTokenCacheItem = new MsalAccessTokenCacheItem(JsonUtils.ExtractExistingOrEmptyString(j, "target"));
		msalAccessTokenCacheItem.TenantId = JsonUtils.ExtractExistingOrEmptyString(j, "realm");
		msalAccessTokenCacheItem.CachedAt = num.ToString(CultureInfo.InvariantCulture);
		msalAccessTokenCacheItem.ExpiresOnUnixTimestamp = num2.ToString(CultureInfo.InvariantCulture);
		msalAccessTokenCacheItem.ExtendedExpiresOnUnixTimestamp = num4.ToString(CultureInfo.InvariantCulture);
		msalAccessTokenCacheItem.RefreshOnUnixTimestamp = JsonUtils.ExtractExistingOrDefault<string>(j, "refresh_on");
		msalAccessTokenCacheItem.UserAssertionHash = JsonUtils.ExtractExistingOrEmptyString(j, "user_assertion_hash");
		msalAccessTokenCacheItem.KeyId = JsonUtils.ExtractExistingOrDefault<string>(j, "kid");
		msalAccessTokenCacheItem.TokenType = JsonUtils.ExtractExistingOrDefault<string>(j, "token_type") ?? "Bearer";
		msalAccessTokenCacheItem.PopulateFieldsFromJObject(j);
		return msalAccessTokenCacheItem;
	}

	internal override JObject ToJObject()
	{
		JObject jObject = base.ToJObject();
		SetItemIfValueNotNull(jObject, "realm", TenantId);
		SetItemIfValueNotNull(jObject, "target", _scopes);
		SetItemIfValueNotNull(jObject, "user_assertion_hash", UserAssertionHash);
		SetItemIfValueNotNull(jObject, "cached_at", CachedAt);
		SetItemIfValueNotNull(jObject, "expires_on", ExpiresOnUnixTimestamp);
		SetItemIfValueNotNull(jObject, "extended_expires_on", ExtendedExpiresOnUnixTimestamp);
		SetItemIfValueNotNull(jObject, "kid", KeyId);
		SetItemIfValueNotNullOrDefault(jObject, "token_type", TokenType, "Bearer");
		SetItemIfValueNotNull(jObject, "refresh_on", RefreshOnUnixTimestamp);
		SetItemIfValueNotNull(jObject, "ext_expires_on", ExtendedExpiresOnUnixTimestamp);
		return jObject;
	}

	internal string ToJsonString()
	{
		return ToJObject().ToString();
	}

	internal MsalAccessTokenCacheKey GetKey()
	{
		return new MsalAccessTokenCacheKey(base.Environment, TenantId, base.HomeAccountId, base.ClientId, _scopes, TokenType);
	}

	internal MsalIdTokenCacheKey GetIdTokenItemKey()
	{
		return new MsalIdTokenCacheKey(base.Environment, TenantId, base.HomeAccountId, base.ClientId);
	}

	internal bool NeedsRefresh()
	{
		if (RefreshOn.HasValue)
		{
			return RefreshOn.Value < DateTime.UtcNow;
		}
		return false;
	}
}
