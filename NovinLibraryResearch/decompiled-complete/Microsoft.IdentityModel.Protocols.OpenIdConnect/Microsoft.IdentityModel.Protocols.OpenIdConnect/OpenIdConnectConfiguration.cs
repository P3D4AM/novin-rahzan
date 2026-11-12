using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

[JsonObject]
public class OpenIdConnectConfiguration
{
	[JsonExtensionData]
	public virtual IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "acr_values_supported", Required = Required.Default)]
	public ICollection<string> AcrValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "authorization_endpoint", Required = Required.Default)]
	public string AuthorizationEndpoint { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "check_session_iframe", Required = Required.Default)]
	public string CheckSessionIframe { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "claims_supported", Required = Required.Default)]
	public ICollection<string> ClaimsSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "claims_locales_supported", Required = Required.Default)]
	public ICollection<string> ClaimsLocalesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "claims_parameter_supported", Required = Required.Default)]
	public bool ClaimsParameterSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "claim_types_supported", Required = Required.Default)]
	public ICollection<string> ClaimTypesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "display_values_supported", Required = Required.Default)]
	public ICollection<string> DisplayValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "end_session_endpoint", Required = Required.Default)]
	public string EndSessionEndpoint { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "frontchannel_logout_session_supported", Required = Required.Default)]
	public string FrontchannelLogoutSessionSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "frontchannel_logout_supported", Required = Required.Default)]
	public string FrontchannelLogoutSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "grant_types_supported", Required = Required.Default)]
	public ICollection<string> GrantTypesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "http_logout_supported", Required = Required.Default)]
	public bool HttpLogoutSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "id_token_encryption_alg_values_supported", Required = Required.Default)]
	public ICollection<string> IdTokenEncryptionAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "id_token_encryption_enc_values_supported", Required = Required.Default)]
	public ICollection<string> IdTokenEncryptionEncValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "id_token_signing_alg_values_supported", Required = Required.Default)]
	public ICollection<string> IdTokenSigningAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "introspection_endpoint", Required = Required.Default)]
	public string IntrospectionEndpoint { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "introspection_endpoint_auth_methods_supported", Required = Required.Default)]
	public ICollection<string> IntrospectionEndpointAuthMethodsSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "introspection_endpoint_auth_signing_alg_values_supported", Required = Required.Default)]
	public ICollection<string> IntrospectionEndpointAuthSigningAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "issuer", Required = Required.Default)]
	public string Issuer { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "jwks_uri", Required = Required.Default)]
	public string JwksUri { get; set; }

	public JsonWebKeySet JsonWebKeySet { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "logout_session_supported", Required = Required.Default)]
	public bool LogoutSessionSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "op_policy_uri", Required = Required.Default)]
	public string OpPolicyUri { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "op_tos_uri", Required = Required.Default)]
	public string OpTosUri { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "registration_endpoint", Required = Required.Default)]
	public string RegistrationEndpoint { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "request_object_encryption_alg_values_supported", Required = Required.Default)]
	public ICollection<string> RequestObjectEncryptionAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "request_object_encryption_enc_values_supported", Required = Required.Default)]
	public ICollection<string> RequestObjectEncryptionEncValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "request_object_signing_alg_values_supported", Required = Required.Default)]
	public ICollection<string> RequestObjectSigningAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "request_parameter_supported", Required = Required.Default)]
	public bool RequestParameterSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "request_uri_parameter_supported", Required = Required.Default)]
	public bool RequestUriParameterSupported { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "require_request_uri_registration", Required = Required.Default)]
	public bool RequireRequestUriRegistration { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "response_modes_supported", Required = Required.Default)]
	public ICollection<string> ResponseModesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "response_types_supported", Required = Required.Default)]
	public ICollection<string> ResponseTypesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "service_documentation", Required = Required.Default)]
	public string ServiceDocumentation { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "scopes_supported", Required = Required.Default)]
	public ICollection<string> ScopesSupported { get; } = new Collection<string>();


	[JsonIgnore]
	public ICollection<SecurityKey> SigningKeys { get; } = new Collection<SecurityKey>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "subject_types_supported", Required = Required.Default)]
	public ICollection<string> SubjectTypesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "token_endpoint", Required = Required.Default)]
	public string TokenEndpoint { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "token_endpoint_auth_methods_supported", Required = Required.Default)]
	public ICollection<string> TokenEndpointAuthMethodsSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "token_endpoint_auth_signing_alg_values_supported", Required = Required.Default)]
	public ICollection<string> TokenEndpointAuthSigningAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "ui_locales_supported", Required = Required.Default)]
	public ICollection<string> UILocalesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "userinfo_endpoint", Required = Required.Default)]
	public string UserInfoEndpoint { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "userinfo_encryption_alg_values_supported", Required = Required.Default)]
	public ICollection<string> UserInfoEndpointEncryptionAlgValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "userinfo_encryption_enc_values_supported", Required = Required.Default)]
	public ICollection<string> UserInfoEndpointEncryptionEncValuesSupported { get; } = new Collection<string>();


	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = "userinfo_signing_alg_values_supported", Required = Required.Default)]
	public ICollection<string> UserInfoEndpointSigningAlgValuesSupported { get; } = new Collection<string>();


	public static OpenIdConnectConfiguration Create(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		LogHelper.LogVerbose("IDX21808: Deserializing json into OpenIdConnectConfiguration object: '{0}'.", json);
		return new OpenIdConnectConfiguration(json);
	}

	public static string Write(OpenIdConnectConfiguration configuration)
	{
		if (configuration == null)
		{
			throw LogHelper.LogArgumentNullException("configuration");
		}
		LogHelper.LogVerbose("IDX21809: Serializing OpenIdConfiguration object to json string.");
		return JsonConvert.SerializeObject(configuration);
	}

	public OpenIdConnectConfiguration()
	{
	}

	public OpenIdConnectConfiguration(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			throw LogHelper.LogArgumentNullException("json");
		}
		try
		{
			LogHelper.LogVerbose("IDX21806: Deserializing json string into json web keys.", json, this);
			JsonConvert.PopulateObject(json, this);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX21815: Error deserializing json: '{0}' into '{1}'.", json, GetType()), innerException));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAcrValuesSupported()
	{
		return AcrValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeClaimsSupported()
	{
		return ClaimsSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeClaimsLocalesSupported()
	{
		return ClaimsLocalesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeClaimTypesSupported()
	{
		return ClaimTypesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisplayValuesSupported()
	{
		return DisplayValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeGrantTypesSupported()
	{
		return GrantTypesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIdTokenEncryptionAlgValuesSupported()
	{
		return IdTokenEncryptionAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIdTokenEncryptionEncValuesSupported()
	{
		return IdTokenEncryptionEncValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIdTokenSigningAlgValuesSupported()
	{
		return IdTokenSigningAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIntrospectionEndpointAuthMethodsSupported()
	{
		return IntrospectionEndpointAuthMethodsSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIntrospectionEndpointAuthSigningAlgValuesSupported()
	{
		return IntrospectionEndpointAuthSigningAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRequestObjectEncryptionAlgValuesSupported()
	{
		return RequestObjectEncryptionAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRequestObjectEncryptionEncValuesSupported()
	{
		return RequestObjectEncryptionEncValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRequestObjectSigningAlgValuesSupported()
	{
		return RequestObjectSigningAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeResponseModesSupported()
	{
		return ResponseModesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeResponseTypesSupported()
	{
		return ResponseTypesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSigningKeys()
	{
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeScopesSupported()
	{
		return ScopesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSubjectTypesSupported()
	{
		return SubjectTypesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTokenEndpointAuthMethodsSupported()
	{
		return TokenEndpointAuthMethodsSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTokenEndpointAuthSigningAlgValuesSupported()
	{
		return TokenEndpointAuthSigningAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeUILocalesSupported()
	{
		return UILocalesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeUserInfoEndpointEncryptionAlgValuesSupported()
	{
		return UserInfoEndpointEncryptionAlgValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeUserInfoEndpointEncryptionEncValuesSupported()
	{
		return UserInfoEndpointEncryptionEncValuesSupported.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeUserInfoEndpointSigningAlgValuesSupported()
	{
		return UserInfoEndpointSigningAlgValuesSupported.Count > 0;
	}
}
