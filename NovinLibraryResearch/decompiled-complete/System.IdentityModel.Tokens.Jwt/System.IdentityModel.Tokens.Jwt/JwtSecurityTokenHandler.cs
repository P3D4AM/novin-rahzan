using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandler : SecurityTokenHandler
{
	private delegate bool CertMatcher(X509Certificate2 cert);

	private ISet<string> _inboundClaimFilter;

	private IDictionary<string, string> _inboundClaimTypeMap;

	private static string _jsonClaimType;

	private const string _namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties";

	private IDictionary<string, string> _outboundClaimTypeMap;

	private IDictionary<string, string> _outboundAlgorithmMap;

	private static string _shortClaimType;

	private bool _mapInboundClaims = DefaultMapInboundClaims;

	public static IDictionary<string, string> DefaultInboundClaimTypeMap;

	public static bool DefaultMapInboundClaims;

	public static IDictionary<string, string> DefaultOutboundClaimTypeMap;

	public static ISet<string> DefaultInboundClaimFilter;

	public static IDictionary<string, string> DefaultOutboundAlgorithmMap;

	public bool MapInboundClaims
	{
		get
		{
			return _mapInboundClaims;
		}
		set
		{
			if (!_mapInboundClaims && value && _inboundClaimTypeMap.Count == 0)
			{
				_inboundClaimTypeMap = new Dictionary<string, string>(DefaultInboundClaimTypeMap);
			}
			_mapInboundClaims = value;
		}
	}

	public IDictionary<string, string> InboundClaimTypeMap
	{
		get
		{
			return _inboundClaimTypeMap;
		}
		set
		{
			_inboundClaimTypeMap = value ?? throw LogHelper.LogArgumentNullException("value");
		}
	}

	public IDictionary<string, string> OutboundClaimTypeMap
	{
		get
		{
			return _outboundClaimTypeMap;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("value");
			}
			_outboundClaimTypeMap = value;
		}
	}

	public IDictionary<string, string> OutboundAlgorithmMap => _outboundAlgorithmMap;

	public ISet<string> InboundClaimFilter
	{
		get
		{
			return _inboundClaimFilter;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("value");
			}
			_inboundClaimFilter = value;
		}
	}

	public static string ShortClaimTypeProperty
	{
		get
		{
			return _shortClaimType;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw LogHelper.LogArgumentNullException("value");
			}
			_shortClaimType = value;
		}
	}

	public static string JsonClaimTypeProperty
	{
		get
		{
			return _jsonClaimType;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw LogHelper.LogArgumentNullException("value");
			}
			_jsonClaimType = value;
		}
	}

	public override bool CanValidateToken => true;

	public override bool CanWriteToken => true;

	public override Type TokenType => typeof(JwtSecurityToken);

	static JwtSecurityTokenHandler()
	{
		_jsonClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/json_type";
		_shortClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/ShortTypeName";
		DefaultInboundClaimTypeMap = ClaimTypeMapping.InboundClaimTypeMap;
		DefaultMapInboundClaims = true;
		DefaultOutboundClaimTypeMap = ClaimTypeMapping.OutboundClaimTypeMap;
		DefaultInboundClaimFilter = ClaimTypeMapping.InboundClaimFilter;
		DefaultOutboundAlgorithmMap = new Dictionary<string, string>
		{
			{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256", "ES256" },
			{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384", "ES384" },
			{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512", "ES512" },
			{ "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "HS256" },
			{ "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384", "HS384" },
			{ "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512", "HS512" },
			{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "RS256" },
			{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", "RS384" },
			{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", "RS512" }
		};
		LogHelper.LogVerbose("Assembly version info: " + typeof(JwtSecurityTokenHandler).AssemblyQualifiedName, Array.Empty<object>());
	}

	public JwtSecurityTokenHandler()
	{
		if (_mapInboundClaims)
		{
			_inboundClaimTypeMap = new Dictionary<string, string>(DefaultInboundClaimTypeMap);
		}
		else
		{
			_inboundClaimTypeMap = new Dictionary<string, string>();
		}
		_outboundClaimTypeMap = new Dictionary<string, string>(DefaultOutboundClaimTypeMap);
		_inboundClaimFilter = new HashSet<string>(DefaultInboundClaimFilter);
		_outboundAlgorithmMap = new Dictionary<string, string>(DefaultOutboundAlgorithmMap);
	}

	public override bool CanReadToken(string token)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			return false;
		}
		if (token.Length > ((TokenHandler)this).MaximumTokenSizeInBytes)
		{
			LogHelper.LogInformation("IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.", new object[2]
			{
				token.Length,
				((TokenHandler)this).MaximumTokenSizeInBytes
			});
			return false;
		}
		string[] array = token.Split(new char[1] { '.' }, 6);
		if (array.Length == 3)
		{
			return JwtTokenUtilities.RegexJws.IsMatch(token);
		}
		if (array.Length == 5)
		{
			return JwtTokenUtilities.RegexJwe.IsMatch(token);
		}
		LogHelper.LogInformation("IDX12720: Token string does not match the token formats: JWE (header.encryptedKey.iv.ciphertext.tag) or JWS (header.payload.signature)", Array.Empty<object>());
		return false;
	}

	public virtual string CreateEncodedJwt(SecurityTokenDescriptor tokenDescriptor)
	{
		if (tokenDescriptor == null)
		{
			throw LogHelper.LogArgumentNullException("tokenDescriptor");
		}
		return CreateJwtSecurityToken(tokenDescriptor).RawData;
	}

	public virtual string CreateEncodedJwt(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt, SigningCredentials signingCredentials)
	{
		return CreateJwtSecurityTokenPrivate(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials, null, null, null).RawData;
	}

	public virtual string CreateEncodedJwt(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
	{
		return CreateJwtSecurityTokenPrivate(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials, encryptingCredentials, null, null).RawData;
	}

	public virtual string CreateEncodedJwt(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, IDictionary<string, object> claimCollection)
	{
		return CreateJwtSecurityTokenPrivate(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials, encryptingCredentials, claimCollection, null).RawData;
	}

	public virtual JwtSecurityToken CreateJwtSecurityToken(SecurityTokenDescriptor tokenDescriptor)
	{
		if (tokenDescriptor == null)
		{
			throw LogHelper.LogArgumentNullException("tokenDescriptor");
		}
		return CreateJwtSecurityTokenPrivate(tokenDescriptor.Issuer, tokenDescriptor.Audience, tokenDescriptor.Subject, tokenDescriptor.NotBefore, tokenDescriptor.Expires, tokenDescriptor.IssuedAt, tokenDescriptor.SigningCredentials, tokenDescriptor.EncryptingCredentials, tokenDescriptor.Claims, tokenDescriptor.TokenType);
	}

	public virtual JwtSecurityToken CreateJwtSecurityToken(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
	{
		return CreateJwtSecurityTokenPrivate(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials, encryptingCredentials, null, null);
	}

	public virtual JwtSecurityToken CreateJwtSecurityToken(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, IDictionary<string, object> claimCollection)
	{
		return CreateJwtSecurityTokenPrivate(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials, encryptingCredentials, claimCollection, null);
	}

	public virtual JwtSecurityToken CreateJwtSecurityToken(string issuer = null, string audience = null, ClaimsIdentity subject = null, DateTime? notBefore = null, DateTime? expires = null, DateTime? issuedAt = null, SigningCredentials signingCredentials = null)
	{
		return CreateJwtSecurityTokenPrivate(issuer, audience, subject, notBefore, expires, issuedAt, signingCredentials, null, null, null);
	}

	public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
	{
		if (tokenDescriptor == null)
		{
			throw LogHelper.LogArgumentNullException("tokenDescriptor");
		}
		return (SecurityToken)(object)CreateJwtSecurityTokenPrivate(tokenDescriptor.Issuer, tokenDescriptor.Audience, tokenDescriptor.Subject, tokenDescriptor.NotBefore, tokenDescriptor.Expires, tokenDescriptor.IssuedAt, tokenDescriptor.SigningCredentials, tokenDescriptor.EncryptingCredentials, tokenDescriptor.Claims, tokenDescriptor.TokenType);
	}

	private JwtSecurityToken CreateJwtSecurityTokenPrivate(string issuer, string audience, ClaimsIdentity subject, DateTime? notBefore, DateTime? expires, DateTime? issuedAt, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, IDictionary<string, object> claimCollection, string tokenType)
	{
		if (((TokenHandler)this).SetDefaultTimesOnTokenCreation && (!expires.HasValue || !issuedAt.HasValue || !notBefore.HasValue))
		{
			DateTime utcNow = DateTime.UtcNow;
			if (!expires.HasValue)
			{
				expires = utcNow + TimeSpan.FromMinutes(((TokenHandler)this).TokenLifetimeInMinutes);
			}
			if (!issuedAt.HasValue)
			{
				issuedAt = utcNow;
			}
			if (!notBefore.HasValue)
			{
				notBefore = utcNow;
			}
		}
		LogHelper.LogVerbose("IDX12721: Creating JwtSecurityToken: Issuer: '{0}', Audience: '{1}'", new object[2]
		{
			audience ?? "null",
			issuer ?? "null"
		});
		JwtPayload jwtPayload = new JwtPayload(issuer, audience, (subject == null) ? null : OutboundClaimTypeTransform(subject.Claims), (claimCollection == null) ? null : OutboundClaimTypeTransform(claimCollection), notBefore, expires, issuedAt);
		JwtHeader jwtHeader = new JwtHeader(signingCredentials, OutboundAlgorithmMap, tokenType);
		if (subject?.Actor != null)
		{
			jwtPayload.AddClaim(new Claim("actort", CreateActorValue(subject.Actor)));
		}
		string text = jwtHeader.Base64UrlEncode();
		string text2 = jwtPayload.Base64UrlEncode();
		string text3 = ((signingCredentials == null) ? string.Empty : JwtTokenUtilities.CreateEncodedSignature(text + "." + text2, signingCredentials));
		LogHelper.LogInformation("IDX12722: Creating security token from the header: '{0}', payload: '{1}' and raw signature: '{2}'.", new object[3] { text, text2, text3 });
		if (encryptingCredentials != null)
		{
			return EncryptToken(new JwtSecurityToken(jwtHeader, jwtPayload, text, text2, text3), encryptingCredentials, tokenType);
		}
		return new JwtSecurityToken(jwtHeader, jwtPayload, text, text2, text3);
	}

	private JwtSecurityToken EncryptToken(JwtSecurityToken innerJwt, EncryptingCredentials encryptingCredentials, string tokenType)
	{
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		CryptoProviderFactory val = encryptingCredentials.CryptoProviderFactory ?? encryptingCredentials.Key.CryptoProviderFactory;
		if (val == null)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException("IDX10620: Unable to obtain a CryptoProviderFactory, both EncryptingCredentials.CryptoProviderFactory and EncryptingCredentials.Key.CrypoProviderFactory are null."));
		}
		byte[] array = null;
		SecurityKey securityKey = JwtTokenUtilities.GetSecurityKey(encryptingCredentials, val, ref array);
		AuthenticatedEncryptionProvider val2 = val.CreateAuthenticatedEncryptionProvider(securityKey, encryptingCredentials.Enc);
		try
		{
			if (val2 == null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenEncryptionFailedException("IDX12730: Failed to create the token encryption provider."));
			}
			try
			{
				JwtHeader jwtHeader = new JwtHeader(encryptingCredentials, OutboundAlgorithmMap, tokenType);
				AuthenticatedEncryptionResult val3 = val2.Encrypt(Encoding.UTF8.GetBytes(innerJwt.RawData), Encoding.ASCII.GetBytes(jwtHeader.Base64UrlEncode()));
				return "dir".Equals(encryptingCredentials.Alg, StringComparison.Ordinal) ? new JwtSecurityToken(jwtHeader, innerJwt, jwtHeader.Base64UrlEncode(), string.Empty, Base64UrlEncoder.Encode(val3.IV), Base64UrlEncoder.Encode(val3.Ciphertext), Base64UrlEncoder.Encode(val3.AuthenticationTag)) : new JwtSecurityToken(jwtHeader, innerJwt, jwtHeader.Base64UrlEncode(), Base64UrlEncoder.Encode(array), Base64UrlEncoder.Encode(val3.IV), Base64UrlEncoder.Encode(val3.Ciphertext), Base64UrlEncoder.Encode(val3.AuthenticationTag));
			}
			catch (Exception ex)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant("IDX10616: Encryption failed. EncryptionProvider failed for: Algorithm: '{0}', SecurityKey: '{1}'. See inner exception.", new object[2] { encryptingCredentials.Enc, encryptingCredentials.Key }), ex));
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private IEnumerable<Claim> OutboundClaimTypeTransform(IEnumerable<Claim> claims)
	{
		foreach (Claim claim in claims)
		{
			string value = null;
			if (_outboundClaimTypeMap.TryGetValue(claim.Type, out value))
			{
				yield return new Claim(value, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer, claim.Subject);
			}
			else
			{
				yield return claim;
			}
		}
	}

	private IDictionary<string, object> OutboundClaimTypeTransform(IDictionary<string, object> claimCollection)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (string key in claimCollection.Keys)
		{
			if (_outboundClaimTypeMap.TryGetValue(key, out var value))
			{
				dictionary[value] = claimCollection[key];
			}
			else
			{
				dictionary[key] = claimCollection[key];
			}
		}
		return dictionary;
	}

	public JwtSecurityToken ReadJwtToken(string token)
	{
		if (string.IsNullOrEmpty(token))
		{
			throw LogHelper.LogArgumentNullException("token");
		}
		if (token.Length > ((TokenHandler)this).MaximumTokenSizeInBytes)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.", new object[2]
			{
				token.Length,
				((TokenHandler)this).MaximumTokenSizeInBytes
			})));
		}
		if (!((SecurityTokenHandler)this).CanReadToken(token))
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12709: CanReadToken() returned false. JWT is not well formed: '{0}'.\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EndcodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.", new object[1] { token })));
		}
		JwtSecurityToken jwtSecurityToken = new JwtSecurityToken();
		jwtSecurityToken.Decode(token.Split(new char[1] { '.' }), token);
		return jwtSecurityToken;
	}

	public override SecurityToken ReadToken(string token)
	{
		return (SecurityToken)(object)ReadJwtToken(token);
	}

	public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters)
	{
		throw new NotImplementedException();
	}

	public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			throw LogHelper.LogArgumentNullException("token");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (token.Length > ((TokenHandler)this).MaximumTokenSizeInBytes)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.", new object[2]
			{
				token.Length,
				((TokenHandler)this).MaximumTokenSizeInBytes
			})));
		}
		string[] array = token.Split(new char[1] { '.' }, 6);
		if (array.Length != 3 && array.Length != 5)
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12741: JWT: '{0}' must have three segments (JWS) or five segments (JWE).", new object[1] { token })));
		}
		ClaimsPrincipal claimsPrincipal = null;
		SecurityToken val = null;
		if (array.Length == 5)
		{
			JwtSecurityToken jwtSecurityToken = ReadJwtToken(token);
			string token2 = DecryptToken(jwtSecurityToken, validationParameters);
			JwtSecurityToken jwtToken = (jwtSecurityToken.InnerToken = ValidateSignature(token2, validationParameters));
			val = (SecurityToken)(object)jwtSecurityToken;
			claimsPrincipal = ValidateTokenPayload(jwtToken, validationParameters);
		}
		else
		{
			val = (SecurityToken)(object)ValidateSignature(token, validationParameters);
			claimsPrincipal = ValidateTokenPayload(val as JwtSecurityToken, validationParameters);
		}
		validatedToken = val;
		return claimsPrincipal;
	}

	protected ClaimsPrincipal ValidateTokenPayload(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		DateTime? expires = ((!jwtToken.Payload.Exp.HasValue) ? null : new DateTime?(((SecurityToken)jwtToken).ValidTo));
		DateTime? notBefore = ((!jwtToken.Payload.Nbf.HasValue) ? null : new DateTime?(((SecurityToken)jwtToken).ValidFrom));
		ValidateLifetime(notBefore, expires, jwtToken, validationParameters);
		ValidateAudience(jwtToken.Audiences, jwtToken, validationParameters);
		string issuer = ValidateIssuer(((SecurityToken)jwtToken).Issuer, jwtToken, validationParameters);
		ValidateTokenReplay(expires, jwtToken.RawData, validationParameters);
		if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jwtToken.Actor))
		{
			SecurityToken val = default(SecurityToken);
			((SecurityTokenHandler)this).ValidateToken(jwtToken.Actor, validationParameters.ActorValidationParameters ?? validationParameters, ref val);
		}
		ValidateIssuerSecurityKey(((SecurityToken)jwtToken).SigningKey, jwtToken, validationParameters);
		Validators.ValidateTokenType(jwtToken.Header.Typ, (SecurityToken)(object)jwtToken, validationParameters);
		ClaimsIdentity claimsIdentity = CreateClaimsIdentity(jwtToken, issuer, validationParameters);
		if (validationParameters.SaveSigninToken)
		{
			claimsIdentity.BootstrapContext = jwtToken.RawData;
		}
		LogHelper.LogInformation("IDX10241: Security token validated. token: '{0}'.", new object[1] { jwtToken.RawData });
		return new ClaimsPrincipal(claimsIdentity);
	}

	public override string WriteToken(SecurityToken token)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		if (token == null)
		{
			throw LogHelper.LogArgumentNullException("token");
		}
		if (!(token is JwtSecurityToken { EncodedPayload: var encodedPayload } jwtSecurityToken))
		{
			throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12706: '{0}' can only write SecurityTokens of type: '{1}', 'token' type is: '{2}'.", new object[3]
			{
				((object)this).GetType(),
				typeof(JwtSecurityToken),
				((object)token).GetType()
			}), "token"));
		}
		string text = string.Empty;
		string empty = string.Empty;
		if (jwtSecurityToken.InnerToken != null)
		{
			if (jwtSecurityToken.SigningCredentials != null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenEncryptionFailedException("IDX12736: JwtSecurityToken.SigningCredentials is not supported when JwtSecurityToken.InnerToken is set."));
			}
			if (jwtSecurityToken.InnerToken.Header.EncryptingCredentials != null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenEncryptionFailedException("IDX12737: EncryptingCredentials set on JwtSecurityToken.InnerToken is not supported."));
			}
			if (jwtSecurityToken.Header.EncryptingCredentials == null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenEncryptionFailedException("IDX12735: If JwtSecurityToken.InnerToken != null, then JwtSecurityToken.Header.EncryptingCredentials must be set."));
			}
			if (jwtSecurityToken.InnerToken.SigningCredentials != null)
			{
				text = JwtTokenUtilities.CreateEncodedSignature(jwtSecurityToken.InnerToken.EncodedHeader + "." + jwtSecurityToken.EncodedPayload, jwtSecurityToken.InnerToken.SigningCredentials);
			}
			return EncryptToken(new JwtSecurityToken(jwtSecurityToken.InnerToken.Header, jwtSecurityToken.InnerToken.Payload, jwtSecurityToken.InnerToken.EncodedHeader, encodedPayload, text), jwtSecurityToken.EncryptingCredentials, jwtSecurityToken.InnerToken.Header.Typ).RawData;
		}
		JwtHeader jwtHeader = ((jwtSecurityToken.EncryptingCredentials == null) ? jwtSecurityToken.Header : new JwtHeader(jwtSecurityToken.SigningCredentials));
		empty = jwtHeader.Base64UrlEncode();
		if (jwtSecurityToken.SigningCredentials != null)
		{
			text = JwtTokenUtilities.CreateEncodedSignature(empty + "." + encodedPayload, jwtSecurityToken.SigningCredentials);
		}
		if (jwtSecurityToken.EncryptingCredentials != null)
		{
			return EncryptToken(new JwtSecurityToken(jwtHeader, jwtSecurityToken.Payload, empty, encodedPayload, text), jwtSecurityToken.EncryptingCredentials, jwtSecurityToken.Header.Typ).RawData;
		}
		return empty + "." + encodedPayload + "." + text;
	}

	private static bool ValidateSignature(byte[] encodedBytes, byte[] signature, SecurityKey key, string algorithm, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		Validators.ValidateAlgorithm(algorithm, key, securityToken, validationParameters);
		CryptoProviderFactory val = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
		SignatureProvider val2 = val.CreateForVerifying(key, algorithm);
		if (val2 == null)
		{
			throw LogHelper.LogExceptionMessage((Exception)new InvalidOperationException(LogHelper.FormatInvariant("IDX10636: CryptoProviderFactory.CreateForVerifying returned null for key: '{0}', signatureAlgorithm: '{1}'.", new object[2]
			{
				(key == null) ? "Null" : ((object)key).ToString(),
				algorithm ?? "Null"
			})));
		}
		try
		{
			return val2.Verify(encodedBytes, signature);
		}
		finally
		{
			val.ReleaseSignatureProvider(val2);
		}
	}

	protected virtual JwtSecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)
	{
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		if (string.IsNullOrWhiteSpace(token))
		{
			throw LogHelper.LogArgumentNullException("token");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.SignatureValidator != null)
		{
			SecurityToken val = validationParameters.SignatureValidator.Invoke(token, validationParameters);
			if (val == null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10505: Signature validation failed. The user defined 'Delegate' specified on TokenValidationParameters returned null when validating token: '{0}'.", new object[1] { token })));
			}
			if (!(val is JwtSecurityToken result))
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10506: Signature validation failed. The user defined 'Delegate' specified on TokenValidationParameters did not return a '{0}', but returned a '{1}' when validating token: '{2}'.", new object[3]
				{
					typeof(JwtSecurityToken),
					((object)val).GetType(),
					token
				})));
			}
			return result;
		}
		JwtSecurityToken jwtSecurityToken = null;
		if (validationParameters.TokenReader != null)
		{
			SecurityToken val2 = validationParameters.TokenReader.Invoke(token, validationParameters);
			if (val2 == null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10510: Signature validation failed. The user defined 'Delegate' specified in TokenValidationParameters returned null when reading token: '{0}'.", new object[1] { token })));
			}
			jwtSecurityToken = val2 as JwtSecurityToken;
			if (jwtSecurityToken == null)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10509: Signature validation failed. The user defined 'Delegate' specified in TokenValidationParameters did not return a '{0}', but returned a '{1}' when reading token: '{2}'.", new object[3]
				{
					typeof(JwtSecurityToken),
					((object)val2).GetType(),
					token
				})));
			}
		}
		else
		{
			jwtSecurityToken = ReadJwtToken(token);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload);
		if (string.IsNullOrEmpty(jwtSecurityToken.RawSignature))
		{
			if (validationParameters.RequireSignedTokens)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10504: Unable to validate signature, token does not have a signature: '{0}'.", new object[1] { token })));
			}
			return jwtSecurityToken;
		}
		bool flag = false;
		IEnumerable<SecurityKey> enumerable = null;
		if (validationParameters.IssuerSigningKeyResolver != null)
		{
			enumerable = validationParameters.IssuerSigningKeyResolver.Invoke(token, (SecurityToken)(object)jwtSecurityToken, jwtSecurityToken.Header.Kid, validationParameters);
		}
		else
		{
			SecurityKey val3 = ResolveIssuerSigningKey(token, jwtSecurityToken, validationParameters);
			if (val3 != null)
			{
				flag = true;
				enumerable = new List<SecurityKey> { val3 };
			}
		}
		if (enumerable == null && validationParameters.TryAllIssuerSigningKeys)
		{
			enumerable = TokenUtilities.GetAllSigningKeys(validationParameters);
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		bool flag2 = !string.IsNullOrEmpty(jwtSecurityToken.Header.Kid);
		byte[] signature;
		try
		{
			signature = Base64UrlEncoder.DecodeBytes(jwtSecurityToken.RawSignature);
		}
		catch (FormatException ex)
		{
			throw new SecurityTokenInvalidSignatureException("IDX10508: Signature validation failed. Signature is improperly formatted.", (Exception)ex);
		}
		if (enumerable != null)
		{
			foreach (SecurityKey item in enumerable)
			{
				try
				{
					if (ValidateSignature(bytes, signature, item, jwtSecurityToken.Header.Alg, (SecurityToken)(object)jwtSecurityToken, validationParameters))
					{
						LogHelper.LogInformation("IDX10242: Security token: '{0}' has a valid signature.", new object[1] { token });
						((SecurityToken)jwtSecurityToken).SigningKey = item;
						return jwtSecurityToken;
					}
				}
				catch (Exception ex2)
				{
					stringBuilder.AppendLine(ex2.ToString());
				}
				if (item != null)
				{
					stringBuilder2.AppendLine(((object)item).ToString() + " , KeyId: " + item.KeyId);
					if (flag2 && !flag && item.KeyId != null)
					{
						flag = jwtSecurityToken.Header.Kid.Equals(item.KeyId, (item is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
					}
				}
			}
		}
		if (flag2)
		{
			if (flag)
			{
				throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10511: Signature validation failed. Keys tried: '{0}'. \nkid: '{1}'. \nExceptions caught:\n '{2}'.\ntoken: '{3}'.", new object[4]
				{
					stringBuilder2,
					jwtSecurityToken.Header.Kid,
					stringBuilder,
					jwtSecurityToken
				})));
			}
			throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant("IDX10501: Signature validation failed. Unable to match key: \nkid: '{0}'.\nExceptions caught:\n '{1}'. \ntoken: '{2}'.", new object[3]
			{
				jwtSecurityToken.Header.Kid,
				stringBuilder,
				jwtSecurityToken
			})));
		}
		if (stringBuilder2.Length > 0)
		{
			throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant("IDX10503: Signature validation failed. Token does not have a kid. Keys tried: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'.", new object[3] { stringBuilder2, stringBuilder, jwtSecurityToken })));
		}
		throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenSignatureKeyNotFoundException("IDX10500: Signature validation failed. No security keys were provided to validate the signature."));
	}

	private static IEnumerable<SecurityKey> GetAllDecryptionKeys(TokenValidationParameters validationParameters)
	{
		if (validationParameters.TokenDecryptionKey != null)
		{
			yield return validationParameters.TokenDecryptionKey;
		}
		if (validationParameters.TokenDecryptionKeys == null)
		{
			yield break;
		}
		foreach (SecurityKey tokenDecryptionKey in validationParameters.TokenDecryptionKeys)
		{
			yield return tokenDecryptionKey;
		}
	}

	protected virtual ClaimsIdentity CreateClaimsIdentity(JwtSecurityToken jwtToken, string issuer, TokenValidationParameters validationParameters)
	{
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		string actualIssuer = issuer;
		if (string.IsNullOrWhiteSpace(issuer))
		{
			LogHelper.LogVerbose("IDX10244: Issuer is null or empty. Using runtime default for creating claims '{0}'.", new object[1] { "LOCAL AUTHORITY" });
			actualIssuer = "LOCAL AUTHORITY";
		}
		if (!MapInboundClaims)
		{
			return CreateClaimsIdentityWithoutMapping(jwtToken, actualIssuer, validationParameters);
		}
		return CreateClaimsIdentityWithMapping(jwtToken, actualIssuer, validationParameters);
	}

	private ClaimsIdentity CreateClaimsIdentityWithMapping(JwtSecurityToken jwtToken, string actualIssuer, TokenValidationParameters validationParameters)
	{
		ClaimsIdentity claimsIdentity = validationParameters.CreateClaimsIdentity((SecurityToken)(object)jwtToken, actualIssuer);
		foreach (Claim claim2 in jwtToken.Claims)
		{
			if (_inboundClaimFilter.Contains(claim2.Type))
			{
				continue;
			}
			bool flag = true;
			if (!_inboundClaimTypeMap.TryGetValue(claim2.Type, out var value))
			{
				value = claim2.Type;
				flag = false;
			}
			if (value == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
			{
				if (claimsIdentity.Actor != null)
				{
					throw LogHelper.LogExceptionMessage((Exception)new InvalidOperationException(LogHelper.FormatInvariant("IDX12710: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'", new object[2] { "actort", claim2.Value })));
				}
				if (((SecurityTokenHandler)this).CanReadToken(claim2.Value))
				{
					JwtSecurityToken jwtToken2 = ((SecurityTokenHandler)this).ReadToken(claim2.Value) as JwtSecurityToken;
					claimsIdentity.Actor = CreateClaimsIdentity(jwtToken2, actualIssuer, validationParameters);
				}
			}
			Claim claim = new Claim(value, claim2.Value, claim2.ValueType, actualIssuer, actualIssuer, claimsIdentity);
			if (claim2.Properties.Count > 0)
			{
				foreach (KeyValuePair<string, string> property in claim2.Properties)
				{
					claim.Properties[property.Key] = property.Value;
				}
			}
			if (flag)
			{
				claim.Properties[ShortClaimTypeProperty] = claim2.Type;
			}
			claimsIdentity.AddClaim(claim);
		}
		return claimsIdentity;
	}

	private ClaimsIdentity CreateClaimsIdentityWithoutMapping(JwtSecurityToken jwtToken, string actualIssuer, TokenValidationParameters validationParameters)
	{
		ClaimsIdentity claimsIdentity = validationParameters.CreateClaimsIdentity((SecurityToken)(object)jwtToken, actualIssuer);
		foreach (Claim claim2 in jwtToken.Claims)
		{
			if (_inboundClaimFilter.Contains(claim2.Type))
			{
				continue;
			}
			string type = claim2.Type;
			if (type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
			{
				if (claimsIdentity.Actor != null)
				{
					throw LogHelper.LogExceptionMessage((Exception)new InvalidOperationException(LogHelper.FormatInvariant("IDX12710: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'", new object[2] { "actort", claim2.Value })));
				}
				if (((SecurityTokenHandler)this).CanReadToken(claim2.Value))
				{
					JwtSecurityToken jwtToken2 = ((SecurityTokenHandler)this).ReadToken(claim2.Value) as JwtSecurityToken;
					claimsIdentity.Actor = CreateClaimsIdentity(jwtToken2, actualIssuer, validationParameters);
				}
			}
			Claim claim = new Claim(type, claim2.Value, claim2.ValueType, actualIssuer, actualIssuer, claimsIdentity);
			if (claim2.Properties.Count > 0)
			{
				foreach (KeyValuePair<string, string> property in claim2.Properties)
				{
					claim.Properties[property.Key] = property.Value;
				}
			}
			claimsIdentity.AddClaim(claim);
		}
		return claimsIdentity;
	}

	protected virtual string CreateActorValue(ClaimsIdentity actor)
	{
		if (actor == null)
		{
			throw LogHelper.LogArgumentNullException("actor");
		}
		if (actor.BootstrapContext != null)
		{
			if (actor.BootstrapContext is string result)
			{
				LogHelper.LogVerbose("IDX12713: Creating actor value using actor.BootstrapContext(as string)", Array.Empty<object>());
				return result;
			}
			if (actor.BootstrapContext is JwtSecurityToken jwtSecurityToken)
			{
				if (jwtSecurityToken.RawData != null)
				{
					LogHelper.LogVerbose("IDX12714: Creating actor value using actor.BootstrapContext.rawData", Array.Empty<object>());
					return jwtSecurityToken.RawData;
				}
				LogHelper.LogVerbose("IDX12715: Creating actor value by writing the JwtSecurityToken created from actor.BootstrapContext", Array.Empty<object>());
				return ((SecurityTokenHandler)this).WriteToken((SecurityToken)(object)jwtSecurityToken);
			}
			LogHelper.LogVerbose("IDX12711: actor.BootstrapContext is not a string AND actor.BootstrapContext is not a JWT", Array.Empty<object>());
		}
		LogHelper.LogVerbose("IDX12712: actor.BootstrapContext is null. Creating the token using actor.Claims.", Array.Empty<object>());
		return ((SecurityTokenHandler)this).WriteToken((SecurityToken)(object)new JwtSecurityToken(null, null, actor.Claims));
	}

	protected virtual void ValidateAudience(IEnumerable<string> audiences, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		Validators.ValidateAudience(audiences, (SecurityToken)(object)jwtToken, validationParameters);
	}

	protected virtual void ValidateLifetime(DateTime? notBefore, DateTime? expires, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		Validators.ValidateLifetime(notBefore, expires, (SecurityToken)(object)jwtToken, validationParameters);
	}

	protected virtual string ValidateIssuer(string issuer, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		return Validators.ValidateIssuer(issuer, (SecurityToken)(object)jwtToken, validationParameters);
	}

	protected virtual void ValidateTokenReplay(DateTime? expires, string securityToken, TokenValidationParameters validationParameters)
	{
		Validators.ValidateTokenReplay(expires, securityToken, validationParameters);
	}

	protected virtual SecurityKey ResolveIssuerSigningKey(string token, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		return JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Header.Kid, jwtToken.Header.X5t, validationParameters);
	}

	protected virtual SecurityKey ResolveTokenDecryptionKey(string token, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (!string.IsNullOrEmpty(jwtToken.Header.Kid))
		{
			if (validationParameters.TokenDecryptionKey != null && string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.Header.Kid, (validationParameters.TokenDecryptionKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				return validationParameters.TokenDecryptionKey;
			}
			if (validationParameters.TokenDecryptionKeys != null)
			{
				foreach (SecurityKey tokenDecryptionKey2 in validationParameters.TokenDecryptionKeys)
				{
					if (tokenDecryptionKey2 != null && string.Equals(tokenDecryptionKey2.KeyId, jwtToken.Header.Kid, (tokenDecryptionKey2 is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
					{
						return tokenDecryptionKey2;
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(jwtToken.Header.X5t))
		{
			if (validationParameters.TokenDecryptionKey != null)
			{
				if (string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.Header.X5t, (validationParameters.TokenDecryptionKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
				{
					return validationParameters.TokenDecryptionKey;
				}
				SecurityKey tokenDecryptionKey = validationParameters.TokenDecryptionKey;
				X509SecurityKey val = (X509SecurityKey)(object)((tokenDecryptionKey is X509SecurityKey) ? tokenDecryptionKey : null);
				if (val != null && string.Equals(val.X5t, jwtToken.Header.X5t, StringComparison.OrdinalIgnoreCase))
				{
					return validationParameters.TokenDecryptionKey;
				}
			}
			if (validationParameters.TokenDecryptionKeys != null)
			{
				foreach (SecurityKey tokenDecryptionKey3 in validationParameters.TokenDecryptionKeys)
				{
					if (tokenDecryptionKey3 != null && string.Equals(tokenDecryptionKey3.KeyId, jwtToken.Header.X5t, (tokenDecryptionKey3 is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
					{
						return tokenDecryptionKey3;
					}
					X509SecurityKey val2 = (X509SecurityKey)(object)((tokenDecryptionKey3 is X509SecurityKey) ? tokenDecryptionKey3 : null);
					if (val2 != null && string.Equals(val2.X5t, jwtToken.Header.X5t, StringComparison.OrdinalIgnoreCase))
					{
						return tokenDecryptionKey3;
					}
				}
			}
		}
		return null;
	}

	protected string DecryptToken(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (string.IsNullOrEmpty(jwtToken.Header.Enc))
		{
			throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenException(LogHelper.FormatInvariant("IDX10612: Decryption failed. Header.Enc is null or empty, it must be specified.", Array.Empty<object>())));
		}
		IEnumerable<SecurityKey> contentEncryptionKeys = GetContentEncryptionKeys(jwtToken, validationParameters);
		return JwtTokenUtilities.DecryptJwtToken((SecurityToken)(object)jwtToken, validationParameters, new JwtTokenDecryptionParameters
		{
			Alg = jwtToken.Header.Alg,
			AuthenticationTag = jwtToken.RawAuthenticationTag,
			Ciphertext = jwtToken.RawCiphertext,
			DecompressionFunction = JwtTokenUtilities.DecompressToken,
			Enc = jwtToken.Header.Enc,
			EncodedHeader = jwtToken.EncodedHeader,
			EncodedToken = jwtToken.RawData,
			InitializationVector = jwtToken.RawInitializationVector,
			Keys = contentEncryptionKeys,
			Zip = jwtToken.Header.Zip
		});
	}

	internal IEnumerable<SecurityKey> GetContentEncryptionKeys(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		IEnumerable<SecurityKey> enumerable = null;
		if (validationParameters.TokenDecryptionKeyResolver != null)
		{
			enumerable = validationParameters.TokenDecryptionKeyResolver.Invoke(jwtToken.RawData, (SecurityToken)(object)jwtToken, jwtToken.Header.Kid, validationParameters);
		}
		else
		{
			SecurityKey val = ResolveTokenDecryptionKey(jwtToken.RawData, jwtToken, validationParameters);
			if (val != null)
			{
				enumerable = new List<SecurityKey> { val };
			}
		}
		if (enumerable == null)
		{
			enumerable = GetAllDecryptionKeys(validationParameters);
		}
		if (jwtToken.Header.Alg.Equals("dir", StringComparison.Ordinal))
		{
			return enumerable;
		}
		List<SecurityKey> list = new List<SecurityKey>();
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (SecurityKey item in enumerable)
		{
			try
			{
				if (item.CryptoProviderFactory.IsSupportedAlgorithm(jwtToken.Header.Alg, item))
				{
					byte[] array = item.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(item, jwtToken.Header.Alg).UnwrapKey(Base64UrlEncoder.DecodeBytes(jwtToken.RawEncryptedKey));
					list.Add((SecurityKey)new SymmetricSecurityKey(array));
				}
			}
			catch (Exception ex)
			{
				stringBuilder.AppendLine(ex.ToString());
			}
			stringBuilder2.AppendLine(((object)item).ToString());
		}
		if (list.Count > 0 || stringBuilder.Length == 0)
		{
			return list;
		}
		throw LogHelper.LogExceptionMessage((Exception)new SecurityTokenKeyWrapException(LogHelper.FormatInvariant("IDX10618: Key unwrap failed using decryption Keys: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'.", new object[3] { stringBuilder2, stringBuilder, jwtToken })));
	}

	private static byte[] GetSymmetricSecurityKey(SecurityKey key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		SymmetricSecurityKey val = (SymmetricSecurityKey)(object)((key is SymmetricSecurityKey) ? key : null);
		if (val != null)
		{
			return val.Key;
		}
		JsonWebKey val2 = (JsonWebKey)(object)((key is JsonWebKey) ? key : null);
		if (val2 != null && val2.K != null)
		{
			return Base64UrlEncoder.DecodeBytes(val2.K);
		}
		return null;
	}

	protected virtual void ValidateIssuerSecurityKey(SecurityKey key, JwtSecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		Validators.ValidateIssuerSecurityKey(key, (SecurityToken)(object)securityToken, validationParameters);
	}

	public override void WriteToken(XmlWriter writer, SecurityToken token)
	{
		throw new NotImplementedException();
	}
}
