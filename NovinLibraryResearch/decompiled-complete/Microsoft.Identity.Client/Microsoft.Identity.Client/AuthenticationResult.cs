using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Identity.Client.AuthScheme;
using Microsoft.Identity.Client.Cache.Items;

namespace Microsoft.Identity.Client;

public class AuthenticationResult
{
	private readonly IAuthenticationScheme _authenticationScheme;

	public string AccessToken { get; }

	public bool IsExtendedLifeTimeToken { get; }

	public string UniqueId { get; }

	public DateTimeOffset ExpiresOn { get; }

	public DateTimeOffset ExtendedExpiresOn { get; }

	public string TenantId { get; }

	public IAccount Account { get; }

	public string IdToken { get; }

	public IEnumerable<string> Scopes { get; }

	public Guid CorrelationId { get; }

	internal string TokenType { get; }

	public AuthenticationResultMetadata AuthenticationResultMetadata { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use Account instead (See https://aka.ms/msal-net-2-released)", true)]
	public IUser User
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AuthenticationResult(string accessToken, bool isExtendedLifeTimeToken, string uniqueId, DateTimeOffset expiresOn, DateTimeOffset extendedExpiresOn, string tenantId, IAccount account, string idToken, IEnumerable<string> scopes, Guid correlationId, string tokenType = "Bearer", AuthenticationResultMetadata authenticationResultMetadata = null)
	{
		AccessToken = accessToken;
		IsExtendedLifeTimeToken = isExtendedLifeTimeToken;
		UniqueId = uniqueId;
		ExpiresOn = expiresOn;
		ExtendedExpiresOn = extendedExpiresOn;
		TenantId = tenantId;
		Account = account;
		IdToken = idToken;
		Scopes = scopes;
		CorrelationId = correlationId;
		TokenType = tokenType;
		AuthenticationResultMetadata = authenticationResultMetadata;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public AuthenticationResult(string accessToken, bool isExtendedLifeTimeToken, string uniqueId, DateTimeOffset expiresOn, DateTimeOffset extendedExpiresOn, string tenantId, IAccount account, string idToken, IEnumerable<string> scopes, Guid correlationId, AuthenticationResultMetadata authenticationResultMetadata, string tokenType = "Bearer")
		: this(accessToken, isExtendedLifeTimeToken, uniqueId, expiresOn, extendedExpiresOn, tenantId, account, idToken, scopes, correlationId, tokenType, authenticationResultMetadata)
	{
	}

	internal AuthenticationResult(MsalAccessTokenCacheItem msalAccessTokenCacheItem, MsalIdTokenCacheItem msalIdTokenCacheItem, IAuthenticationScheme authenticationScheme, Guid correlationID, TokenSource tokenSource)
	{
		_authenticationScheme = authenticationScheme ?? throw new ArgumentNullException("authenticationScheme");
		string text = msalAccessTokenCacheItem?.HomeAccountId ?? msalIdTokenCacheItem?.HomeAccountId;
		string environment = msalAccessTokenCacheItem?.Environment ?? msalIdTokenCacheItem?.Environment;
		if (text != null)
		{
			string username = null;
			if (msalIdTokenCacheItem != null)
			{
				username = ((!msalIdTokenCacheItem.IsAdfs) ? msalIdTokenCacheItem?.IdToken?.PreferredUsername : msalIdTokenCacheItem?.IdToken.Upn);
			}
			Account = new Account(text, username, environment);
		}
		if (msalAccessTokenCacheItem != null)
		{
			AccessToken = authenticationScheme.FormatAccessToken(msalAccessTokenCacheItem);
			ExpiresOn = msalAccessTokenCacheItem.ExpiresOn;
			ExtendedExpiresOn = msalAccessTokenCacheItem.ExtendedExpiresOn;
			Scopes = msalAccessTokenCacheItem.ScopeSet;
			IsExtendedLifeTimeToken = msalAccessTokenCacheItem.IsExtendedLifeTimeToken;
			TokenType = msalAccessTokenCacheItem.TokenType;
		}
		UniqueId = msalIdTokenCacheItem?.IdToken?.GetUniqueId();
		TenantId = msalIdTokenCacheItem?.IdToken?.TenantId;
		IdToken = msalIdTokenCacheItem?.Secret;
		CorrelationId = correlationID;
		AuthenticationResultMetadata = new AuthenticationResultMetadata(tokenSource);
	}

	internal AuthenticationResult()
	{
	}

	public string CreateAuthorizationHeader()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} {1}", _authenticationScheme?.AuthorizationHeaderPrefix ?? TokenType, AccessToken);
	}
}
