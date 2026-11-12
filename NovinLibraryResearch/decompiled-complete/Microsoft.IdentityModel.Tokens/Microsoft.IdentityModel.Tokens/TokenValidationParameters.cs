using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class TokenValidationParameters
{
	private string _authenticationType;

	private TimeSpan _clockSkew = DefaultClockSkew;

	private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

	private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

	public static readonly string DefaultAuthenticationType = "AuthenticationTypes.Federation";

	public static readonly TimeSpan DefaultClockSkew = TimeSpan.FromSeconds(300.0);

	public const int DefaultMaximumTokenSizeInBytes = 256000;

	public TokenValidationParameters ActorValidationParameters { get; set; }

	public AlgorithmValidator AlgorithmValidator { get; set; }

	public AudienceValidator AudienceValidator { get; set; }

	public string AuthenticationType
	{
		get
		{
			return _authenticationType;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw LogHelper.LogExceptionMessage(new ArgumentNullException("AuthenticationType"));
			}
			_authenticationType = value;
		}
	}

	[DefaultValue(300)]
	public TimeSpan ClockSkew
	{
		get
		{
			return _clockSkew;
		}
		set
		{
			if (value < TimeSpan.Zero)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX10100: ClockSkew must be greater than TimeSpan.Zero. value: '{0}'", value)));
			}
			_clockSkew = value;
		}
	}

	public CryptoProviderFactory CryptoProviderFactory { get; set; }

	[DefaultValue(true)]
	public bool IgnoreTrailingSlashWhenValidatingAudience { get; set; } = true;


	public IssuerSigningKeyValidator IssuerSigningKeyValidator { get; set; }

	public SecurityKey IssuerSigningKey { get; set; }

	public IssuerSigningKeyResolver IssuerSigningKeyResolver { get; set; }

	public IEnumerable<SecurityKey> IssuerSigningKeys { get; set; }

	public IssuerValidator IssuerValidator { get; set; }

	public LifetimeValidator LifetimeValidator { get; set; }

	public string NameClaimType
	{
		get
		{
			return _nameClaimType;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", "IDX10102: NameClaimType cannot be null or whitespace."));
			}
			_nameClaimType = value;
		}
	}

	public Func<SecurityToken, string, string> NameClaimTypeRetriever { get; set; }

	public IDictionary<string, object> PropertyBag { get; set; }

	[DefaultValue(true)]
	public bool RequireAudience { get; set; }

	[DefaultValue(true)]
	public bool RequireExpirationTime { get; set; }

	[DefaultValue(true)]
	public bool RequireSignedTokens { get; set; }

	public string RoleClaimType
	{
		get
		{
			return _roleClaimType;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", "IDX10103: RoleClaimType cannot be null or whitespace."));
			}
			_roleClaimType = value;
		}
	}

	public Func<SecurityToken, string, string> RoleClaimTypeRetriever { get; set; }

	[DefaultValue(false)]
	public bool SaveSigninToken { get; set; }

	public SignatureValidator SignatureValidator { get; set; }

	public SecurityKey TokenDecryptionKey { get; set; }

	public TokenDecryptionKeyResolver TokenDecryptionKeyResolver { get; set; }

	public IEnumerable<SecurityKey> TokenDecryptionKeys { get; set; }

	public TokenReader TokenReader { get; set; }

	public ITokenReplayCache TokenReplayCache { get; set; }

	public TokenReplayValidator TokenReplayValidator { get; set; }

	[DefaultValue(true)]
	public bool TryAllIssuerSigningKeys { get; set; }

	public TypeValidator TypeValidator { get; set; }

	[DefaultValue(false)]
	public bool ValidateActor { get; set; }

	[DefaultValue(true)]
	public bool ValidateAudience { get; set; }

	[DefaultValue(true)]
	public bool ValidateIssuer { get; set; }

	[DefaultValue(false)]
	public bool ValidateIssuerSigningKey { get; set; }

	[DefaultValue(true)]
	public bool ValidateLifetime { get; set; }

	[DefaultValue(false)]
	public bool ValidateTokenReplay { get; set; }

	public IEnumerable<string> ValidAlgorithms { get; set; }

	public string ValidAudience { get; set; }

	public IEnumerable<string> ValidAudiences { get; set; }

	public string ValidIssuer { get; set; }

	public IEnumerable<string> ValidIssuers { get; set; }

	public IEnumerable<string> ValidTypes { get; set; }

	protected TokenValidationParameters(TokenValidationParameters other)
	{
		if (other == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentNullException("other"));
		}
		AlgorithmValidator = other.AlgorithmValidator;
		ActorValidationParameters = other.ActorValidationParameters?.Clone();
		AudienceValidator = other.AudienceValidator;
		_authenticationType = other._authenticationType;
		ClockSkew = other.ClockSkew;
		CryptoProviderFactory = other.CryptoProviderFactory;
		IgnoreTrailingSlashWhenValidatingAudience = other.IgnoreTrailingSlashWhenValidatingAudience;
		IssuerSigningKey = other.IssuerSigningKey;
		IssuerSigningKeyResolver = other.IssuerSigningKeyResolver;
		IssuerSigningKeys = other.IssuerSigningKeys;
		IssuerSigningKeyValidator = other.IssuerSigningKeyValidator;
		IssuerValidator = other.IssuerValidator;
		LifetimeValidator = other.LifetimeValidator;
		NameClaimType = other.NameClaimType;
		NameClaimTypeRetriever = other.NameClaimTypeRetriever;
		PropertyBag = other.PropertyBag;
		RequireAudience = other.RequireAudience;
		RequireExpirationTime = other.RequireExpirationTime;
		RequireSignedTokens = other.RequireSignedTokens;
		RoleClaimType = other.RoleClaimType;
		RoleClaimTypeRetriever = other.RoleClaimTypeRetriever;
		SaveSigninToken = other.SaveSigninToken;
		SignatureValidator = other.SignatureValidator;
		TokenDecryptionKey = other.TokenDecryptionKey;
		TokenDecryptionKeyResolver = other.TokenDecryptionKeyResolver;
		TokenDecryptionKeys = other.TokenDecryptionKeys;
		TokenReader = other.TokenReader;
		TokenReplayCache = other.TokenReplayCache;
		TokenReplayValidator = other.TokenReplayValidator;
		TryAllIssuerSigningKeys = other.TryAllIssuerSigningKeys;
		TypeValidator = other.TypeValidator;
		ValidateActor = other.ValidateActor;
		ValidateAudience = other.ValidateAudience;
		ValidateIssuer = other.ValidateIssuer;
		ValidateIssuerSigningKey = other.ValidateIssuerSigningKey;
		ValidateLifetime = other.ValidateLifetime;
		ValidateTokenReplay = other.ValidateTokenReplay;
		ValidAlgorithms = other.ValidAlgorithms;
		ValidAudience = other.ValidAudience;
		ValidAudiences = other.ValidAudiences;
		ValidIssuer = other.ValidIssuer;
		ValidIssuers = other.ValidIssuers;
		ValidTypes = other.ValidTypes;
	}

	public TokenValidationParameters()
	{
		RequireExpirationTime = true;
		RequireSignedTokens = true;
		RequireAudience = true;
		SaveSigninToken = false;
		TryAllIssuerSigningKeys = true;
		ValidateActor = false;
		ValidateAudience = true;
		ValidateIssuer = true;
		ValidateIssuerSigningKey = false;
		ValidateLifetime = true;
		ValidateTokenReplay = false;
	}

	public virtual TokenValidationParameters Clone()
	{
		return new TokenValidationParameters(this);
	}

	public virtual ClaimsIdentity CreateClaimsIdentity(SecurityToken securityToken, string issuer)
	{
		string text = null;
		text = ((NameClaimTypeRetriever == null) ? NameClaimType : NameClaimTypeRetriever(securityToken, issuer));
		string text2 = null;
		text2 = ((RoleClaimTypeRetriever == null) ? RoleClaimType : RoleClaimTypeRetriever(securityToken, issuer));
		LogHelper.LogInformation("IDX10245: Creating claims identity from the validated token: '{0}'.", securityToken);
		return new ClaimsIdentity(AuthenticationType ?? DefaultAuthenticationType, text ?? "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", text2 ?? "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
	}
}
