using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.OAuth2;

[JsonObject]
[Preserve(AllMembers = true)]
internal class MsalTokenResponse : OAuth2ResponseBase
{
	private long _expiresIn;

	private long _extendedExpiresIn;

	private long _refreshIn;

	[JsonProperty(PropertyName = "token_type")]
	public string TokenType { get; set; }

	[JsonProperty(PropertyName = "access_token")]
	public string AccessToken { get; set; }

	[JsonProperty(PropertyName = "refresh_token")]
	public string RefreshToken { get; set; }

	[JsonProperty(PropertyName = "scope")]
	public string Scope { get; set; }

	[JsonProperty(PropertyName = "client_info")]
	public string ClientInfo { get; set; }

	[JsonProperty(PropertyName = "id_token")]
	public string IdToken { get; set; }

	[JsonProperty(PropertyName = "expires_in")]
	public long ExpiresIn
	{
		get
		{
			return _expiresIn;
		}
		set
		{
			_expiresIn = value;
			AccessTokenExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(_expiresIn);
		}
	}

	[JsonProperty(PropertyName = "ext_expires_in")]
	public long ExtendedExpiresIn
	{
		get
		{
			return _extendedExpiresIn;
		}
		set
		{
			_extendedExpiresIn = value;
			AccessTokenExtendedExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(_extendedExpiresIn);
		}
	}

	[JsonProperty(PropertyName = "refresh_in")]
	public long RefreshIn
	{
		get
		{
			return _refreshIn;
		}
		set
		{
			_refreshIn = value;
			AccessTokenRefreshOn = DateTime.UtcNow + TimeSpan.FromSeconds(_refreshIn);
		}
	}

	[JsonProperty(PropertyName = "foci")]
	public string FamilyId { get; set; }

	public string WamAccountId { get; set; }

	public DateTimeOffset AccessTokenExpiresOn { get; private set; }

	public DateTimeOffset AccessTokenExtendedExpiresOn { get; private set; }

	public DateTimeOffset? AccessTokenRefreshOn { get; private set; }

	public TokenSource TokenSource { get; set; }

	public HttpResponse HttpResponse { get; set; }

	internal static MsalTokenResponse CreateFromiOSBrokerResponse(Dictionary<string, string> responseDictionary)
	{
		if (responseDictionary.TryGetValue("broker_error_code", out var value))
		{
			return new MsalTokenResponse
			{
				Error = value,
				ErrorDescription = CoreHelpers.UrlDecode(responseDictionary["error_description"])
			};
		}
		string value2;
		MsalTokenResponse msalTokenResponse = new MsalTokenResponse
		{
			AccessToken = responseDictionary["access_token"],
			RefreshToken = (responseDictionary.ContainsKey("refresh_token") ? responseDictionary["refresh_token"] : null),
			IdToken = responseDictionary["id_token"],
			TokenType = "Bearer",
			CorrelationId = responseDictionary["correlation_id"],
			Scope = responseDictionary["scope"],
			ExpiresIn = (responseDictionary.TryGetValue("expires_on", out value2) ? CoreHelpers.GetDurationFromNowInSeconds(value2) : 0),
			ClientInfo = (responseDictionary.ContainsKey("client_info") ? responseDictionary["client_info"] : null),
			TokenSource = TokenSource.Broker
		};
		if (responseDictionary.ContainsKey("refresh_in"))
		{
			msalTokenResponse.RefreshIn = long.Parse(responseDictionary["refresh_in"], CultureInfo.InvariantCulture);
		}
		return msalTokenResponse;
	}

	internal static MsalTokenResponse CreateFromAndroidBrokerResponse(string jsonResponse, string correlationId)
	{
		JObject jObject = JObject.Parse(jsonResponse);
		string text = jObject["broker_error_code"]?.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			return new MsalTokenResponse
			{
				Error = text,
				ErrorDescription = jObject["broker_error_message"]?.ToString()
			};
		}
		return new MsalTokenResponse
		{
			AccessToken = jObject["access_token"].ToString(),
			IdToken = jObject["id_token"].ToString(),
			CorrelationId = correlationId,
			Scope = jObject["scopes"].ToString(),
			ExpiresIn = CoreHelpers.GetDurationFromNowInSeconds(jObject["expires_on"].ToString()),
			ExtendedExpiresIn = CoreHelpers.GetDurationFromNowInSeconds(jObject["ext_expires_on"].ToString()),
			ClientInfo = jObject["client_info"].ToString(),
			TokenType = (jObject["token_type"]?.ToString() ?? "Bearer"),
			TokenSource = TokenSource.Broker
		};
	}
}
