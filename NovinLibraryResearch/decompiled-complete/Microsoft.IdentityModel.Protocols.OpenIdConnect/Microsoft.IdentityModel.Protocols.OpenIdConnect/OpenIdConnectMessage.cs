using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectMessage : AuthenticationProtocolMessage
{
	public string AuthorizationEndpoint { get; set; }

	public string AccessToken
	{
		get
		{
			return GetParameter("access_token");
		}
		set
		{
			SetParameter("access_token", value);
		}
	}

	public string AcrValues
	{
		get
		{
			return GetParameter("acr_values");
		}
		set
		{
			SetParameter("acr_values", value);
		}
	}

	public string ClaimsLocales
	{
		get
		{
			return GetParameter("claims_locales");
		}
		set
		{
			SetParameter("claims_locales", value);
		}
	}

	public string ClientAssertion
	{
		get
		{
			return GetParameter("client_assertion");
		}
		set
		{
			SetParameter("client_assertion", value);
		}
	}

	public string ClientAssertionType
	{
		get
		{
			return GetParameter("client_assertion_type");
		}
		set
		{
			SetParameter("client_assertion_type", value);
		}
	}

	public string ClientId
	{
		get
		{
			return GetParameter("client_id");
		}
		set
		{
			SetParameter("client_id", value);
		}
	}

	public string ClientSecret
	{
		get
		{
			return GetParameter("client_secret");
		}
		set
		{
			SetParameter("client_secret", value);
		}
	}

	public string Code
	{
		get
		{
			return GetParameter("code");
		}
		set
		{
			SetParameter("code", value);
		}
	}

	public string Display
	{
		get
		{
			return GetParameter("display");
		}
		set
		{
			SetParameter("display", value);
		}
	}

	public string DomainHint
	{
		get
		{
			return GetParameter("domain_hint");
		}
		set
		{
			SetParameter("domain_hint", value);
		}
	}

	public bool EnableTelemetryParameters { get; set; } = EnableTelemetryParametersByDefault;


	public static bool EnableTelemetryParametersByDefault { get; set; } = true;


	public string Error
	{
		get
		{
			return GetParameter("error");
		}
		set
		{
			SetParameter("error", value);
		}
	}

	public string ErrorDescription
	{
		get
		{
			return GetParameter("error_description");
		}
		set
		{
			SetParameter("error_description", value);
		}
	}

	public string ErrorUri
	{
		get
		{
			return GetParameter("error_uri");
		}
		set
		{
			SetParameter("error_uri", value);
		}
	}

	public string ExpiresIn
	{
		get
		{
			return GetParameter("expires_in");
		}
		set
		{
			SetParameter("expires_in", value);
		}
	}

	public string GrantType
	{
		get
		{
			return GetParameter("grant_type");
		}
		set
		{
			SetParameter("grant_type", value);
		}
	}

	public string IdToken
	{
		get
		{
			return GetParameter("id_token");
		}
		set
		{
			SetParameter("id_token", value);
		}
	}

	public string IdTokenHint
	{
		get
		{
			return GetParameter("id_token_hint");
		}
		set
		{
			SetParameter("id_token_hint", value);
		}
	}

	public string IdentityProvider
	{
		get
		{
			return GetParameter("identity_provider");
		}
		set
		{
			SetParameter("identity_provider", value);
		}
	}

	public string Iss
	{
		get
		{
			return GetParameter("iss");
		}
		set
		{
			SetParameter("iss", value);
		}
	}

	public string LoginHint
	{
		get
		{
			return GetParameter("login_hint");
		}
		set
		{
			SetParameter("login_hint", value);
		}
	}

	public string MaxAge
	{
		get
		{
			return GetParameter("max_age");
		}
		set
		{
			SetParameter("max_age", value);
		}
	}

	public string Nonce
	{
		get
		{
			return GetParameter("nonce");
		}
		set
		{
			SetParameter("nonce", value);
		}
	}

	public string Password
	{
		get
		{
			return GetParameter("password");
		}
		set
		{
			SetParameter("password", value);
		}
	}

	public string PostLogoutRedirectUri
	{
		get
		{
			return GetParameter("post_logout_redirect_uri");
		}
		set
		{
			SetParameter("post_logout_redirect_uri", value);
		}
	}

	public string Prompt
	{
		get
		{
			return GetParameter("prompt");
		}
		set
		{
			SetParameter("prompt", value);
		}
	}

	public string RedirectUri
	{
		get
		{
			return GetParameter("redirect_uri");
		}
		set
		{
			SetParameter("redirect_uri", value);
		}
	}

	public string RefreshToken
	{
		get
		{
			return GetParameter("refresh_token");
		}
		set
		{
			SetParameter("refresh_token", value);
		}
	}

	public OpenIdConnectRequestType RequestType { get; set; }

	public string RequestUri
	{
		get
		{
			return GetParameter("request_uri");
		}
		set
		{
			SetParameter("request_uri", value);
		}
	}

	public string ResponseMode
	{
		get
		{
			return GetParameter("response_mode");
		}
		set
		{
			SetParameter("response_mode", value);
		}
	}

	public string ResponseType
	{
		get
		{
			return GetParameter("response_type");
		}
		set
		{
			SetParameter("response_type", value);
		}
	}

	public string Resource
	{
		get
		{
			return GetParameter("resource");
		}
		set
		{
			SetParameter("resource", value);
		}
	}

	public string Scope
	{
		get
		{
			return GetParameter("scope");
		}
		set
		{
			SetParameter("scope", value);
		}
	}

	public string SessionState
	{
		get
		{
			return GetParameter("session_state");
		}
		set
		{
			SetParameter("session_state", value);
		}
	}

	public string Sid
	{
		get
		{
			return GetParameter("sid");
		}
		set
		{
			SetParameter("sid", value);
		}
	}

	public string SkuTelemetryValue { get; set; } = "ID_NET461";


	public string State
	{
		get
		{
			return GetParameter("state");
		}
		set
		{
			SetParameter("state", value);
		}
	}

	public string TargetLinkUri
	{
		get
		{
			return GetParameter("target_link_uri");
		}
		set
		{
			SetParameter("target_link_uri", value);
		}
	}

	public string TokenEndpoint { get; set; }

	public string TokenType
	{
		get
		{
			return GetParameter("token_type");
		}
		set
		{
			SetParameter("token_type", value);
		}
	}

	public string UiLocales
	{
		get
		{
			return GetParameter("ui_locales");
		}
		set
		{
			SetParameter("ui_locales", value);
		}
	}

	public string UserId
	{
		get
		{
			return GetParameter("user_id");
		}
		set
		{
			SetParameter("user_id", value);
		}
	}

	public string Username
	{
		get
		{
			return GetParameter("username");
		}
		set
		{
			SetParameter("username", value);
		}
	}

	public OpenIdConnectMessage()
	{
	}

	public OpenIdConnectMessage(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		try
		{
			SetJsonParameters(JObject.Parse(json));
		}
		catch
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX21106: Error in deserializing to json: '{0}'", json)));
		}
	}

	protected OpenIdConnectMessage(OpenIdConnectMessage other)
	{
		if (other == null)
		{
			throw LogHelper.LogArgumentNullException("other");
		}
		foreach (KeyValuePair<string, string> parameter in other.Parameters)
		{
			SetParameter(parameter.Key, parameter.Value);
		}
		AuthorizationEndpoint = other.AuthorizationEndpoint;
		base.IssuerAddress = other.IssuerAddress;
		RequestType = other.RequestType;
		TokenEndpoint = other.TokenEndpoint;
		EnableTelemetryParameters = other.EnableTelemetryParameters;
	}

	public OpenIdConnectMessage(NameValueCollection nameValueCollection)
	{
		if (nameValueCollection == null)
		{
			throw LogHelper.LogArgumentNullException("nameValueCollection");
		}
		string[] allKeys = nameValueCollection.AllKeys;
		foreach (string text in allKeys)
		{
			if (text != null)
			{
				SetParameter(text, nameValueCollection[text]);
			}
		}
	}

	public OpenIdConnectMessage(IEnumerable<KeyValuePair<string, string[]>> parameters)
	{
		if (parameters == null)
		{
			throw LogHelper.LogArgumentNullException("parameters");
		}
		foreach (KeyValuePair<string, string[]> parameter in parameters)
		{
			if (parameter.Value == null || string.IsNullOrWhiteSpace(parameter.Key))
			{
				continue;
			}
			string[] value = parameter.Value;
			foreach (string text in value)
			{
				if (text != null)
				{
					SetParameter(parameter.Key, text);
					break;
				}
			}
		}
	}

	[Obsolete("The 'OpenIdConnectMessage(object json)' constructor is obsolete. Please use 'OpenIdConnectMessage(string json)' instead.")]
	public OpenIdConnectMessage(object json)
	{
		if (json == null)
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		JObject jsonParameters = JObject.Parse(json.ToString());
		SetJsonParameters(jsonParameters);
	}

	private void SetJsonParameters(JObject json)
	{
		if (json == null)
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		foreach (KeyValuePair<string, JToken> item in json)
		{
			if (json.TryGetValue(item.Key, out var value))
			{
				SetParameter(item.Key, value.ToString());
			}
		}
	}

	public virtual OpenIdConnectMessage Clone()
	{
		return new OpenIdConnectMessage(this);
	}

	public virtual string CreateAuthenticationRequestUrl()
	{
		OpenIdConnectMessage openIdConnectMessage = Clone();
		openIdConnectMessage.RequestType = OpenIdConnectRequestType.Authentication;
		EnsureTelemetryValues(openIdConnectMessage);
		return openIdConnectMessage.BuildRedirectUrl();
	}

	public virtual string CreateLogoutRequestUrl()
	{
		OpenIdConnectMessage openIdConnectMessage = Clone();
		openIdConnectMessage.RequestType = OpenIdConnectRequestType.Logout;
		EnsureTelemetryValues(openIdConnectMessage);
		return openIdConnectMessage.BuildRedirectUrl();
	}

	private void EnsureTelemetryValues(OpenIdConnectMessage clonedMessage)
	{
		if (EnableTelemetryParameters)
		{
			clonedMessage.SetParameter("x-client-SKU", SkuTelemetryValue);
			clonedMessage.SetParameter("x-client-ver", typeof(OpenIdConnectMessage).GetTypeInfo().Assembly.GetName().Version.ToString());
		}
	}
}
