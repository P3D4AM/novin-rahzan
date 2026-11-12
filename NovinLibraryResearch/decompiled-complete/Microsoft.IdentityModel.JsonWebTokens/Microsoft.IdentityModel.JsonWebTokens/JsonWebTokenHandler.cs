using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens;

public class JsonWebTokenHandler : TokenHandler
{
	public const string Base64UrlEncodedUnsignedJWSHeader = "eyJhbGciOiJub25lIn0";

	public Type TokenType => typeof(JsonWebToken);

	public virtual bool CanValidateToken => true;

	public virtual bool CanReadToken(string token)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			return false;
		}
		if (token.Length > MaximumTokenSizeInBytes)
		{
			LogHelper.LogInformation("IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.", token.Length, MaximumTokenSizeInBytes);
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
		LogHelper.LogInformation("IDX14107: Token string does not match the token formats: JWE (header.encryptedKey.iv.ciphertext.tag) or JWS (header.payload.signature)");
		return false;
	}

	private static JObject CreateDefaultJWEHeader(EncryptingCredentials encryptingCredentials, string compressionAlgorithm, string tokenType)
	{
		JObject jObject = new JObject();
		jObject.Add("alg", encryptingCredentials.Alg);
		jObject.Add("enc", encryptingCredentials.Enc);
		if (!string.IsNullOrEmpty(encryptingCredentials.Key.KeyId))
		{
			jObject.Add("kid", encryptingCredentials.Key.KeyId);
		}
		if (!string.IsNullOrEmpty(compressionAlgorithm))
		{
			jObject.Add("zip", compressionAlgorithm);
		}
		if (string.IsNullOrEmpty(tokenType))
		{
			jObject.Add("typ", "JWT");
		}
		else
		{
			jObject.Add("typ", tokenType);
		}
		return jObject;
	}

	private static JObject CreateDefaultJWSHeader(SigningCredentials signingCredentials, string tokenType)
	{
		JObject jObject = null;
		if (signingCredentials == null)
		{
			jObject = new JObject { { "alg", "none" } };
		}
		else
		{
			jObject = new JObject { { "alg", signingCredentials.Algorithm } };
			if (signingCredentials.Key.KeyId != null)
			{
				jObject.Add("kid", signingCredentials.Key.KeyId);
			}
			if (signingCredentials.Key is X509SecurityKey x509SecurityKey)
			{
				jObject["x5t"] = x509SecurityKey.X5t;
			}
		}
		if (string.IsNullOrEmpty(tokenType))
		{
			jObject.Add("typ", "JWT");
		}
		else
		{
			jObject.Add("typ", tokenType);
		}
		return jObject;
	}

	public virtual string CreateToken(string payload)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		return CreateTokenPrivate(JObject.Parse(payload), null, null, null, null, null);
	}

	public virtual string CreateToken(string payload, IDictionary<string, object> additionalHeaderClaims)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (additionalHeaderClaims == null)
		{
			throw LogHelper.LogArgumentNullException("additionalHeaderClaims");
		}
		return CreateTokenPrivate(JObject.Parse(payload), null, null, null, additionalHeaderClaims, null);
	}

	public virtual string CreateToken(string payload, SigningCredentials signingCredentials)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		return CreateTokenPrivate(JObject.Parse(payload), signingCredentials, null, null, null, null);
	}

	public virtual string CreateToken(string payload, SigningCredentials signingCredentials, IDictionary<string, object> additionalHeaderClaims)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		if (additionalHeaderClaims == null)
		{
			throw LogHelper.LogArgumentNullException("additionalHeaderClaims");
		}
		return CreateTokenPrivate(JObject.Parse(payload), signingCredentials, null, null, additionalHeaderClaims, null);
	}

	public virtual string CreateToken(SecurityTokenDescriptor tokenDescriptor)
	{
		if (tokenDescriptor == null)
		{
			throw LogHelper.LogArgumentNullException("tokenDescriptor");
		}
		if ((tokenDescriptor.Subject == null || !tokenDescriptor.Subject.Claims.Any()) && (tokenDescriptor.Claims == null || !tokenDescriptor.Claims.Any()))
		{
			LogHelper.LogWarning("IDX14114: Both '{0}.{1}' and '{0}.{2}' are null or empty.", "SecurityTokenDescriptor", "Subject", "Claims");
		}
		JObject jObject = ((tokenDescriptor.Subject == null) ? new JObject() : JObject.FromObject(TokenUtilities.CreateDictionaryFromClaims(tokenDescriptor.Subject.Claims)));
		if (tokenDescriptor.Claims != null && tokenDescriptor.Claims.Count > 0)
		{
			jObject.Merge(JObject.FromObject(tokenDescriptor.Claims), new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Replace
			});
		}
		if (tokenDescriptor.Audience != null)
		{
			if (jObject.ContainsKey("aud"))
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.", "Audience"));
			}
			jObject["aud"] = tokenDescriptor.Audience;
		}
		if (tokenDescriptor.Expires.HasValue)
		{
			if (jObject.ContainsKey("exp"))
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.", "Expires"));
			}
			jObject["exp"] = EpochTime.GetIntDate(tokenDescriptor.Expires.Value);
		}
		if (tokenDescriptor.Issuer != null)
		{
			if (jObject.ContainsKey("iss"))
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.", "Issuer"));
			}
			jObject["iss"] = tokenDescriptor.Issuer;
		}
		if (tokenDescriptor.IssuedAt.HasValue)
		{
			if (jObject.ContainsKey("iat"))
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.", "IssuedAt"));
			}
			jObject["iat"] = EpochTime.GetIntDate(tokenDescriptor.IssuedAt.Value);
		}
		if (tokenDescriptor.NotBefore.HasValue)
		{
			if (jObject.ContainsKey("nbf"))
			{
				LogHelper.LogInformation(LogHelper.FormatInvariant("IDX14113: A duplicate value for 'SecurityTokenDescriptor.{0}' exists in 'SecurityTokenDescriptor.Claims'. \nThe value of 'SecurityTokenDescriptor.{0}' is used.", "NotBefore"));
			}
			jObject["nbf"] = EpochTime.GetIntDate(tokenDescriptor.NotBefore.Value);
		}
		return CreateTokenPrivate(jObject, tokenDescriptor.SigningCredentials, tokenDescriptor.EncryptingCredentials, tokenDescriptor.CompressionAlgorithm, tokenDescriptor.AdditionalHeaderClaims, tokenDescriptor.TokenType);
	}

	public virtual string CreateToken(string payload, EncryptingCredentials encryptingCredentials)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		return CreateTokenPrivate(JObject.Parse(payload), null, encryptingCredentials, null, null, null);
	}

	public virtual string CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		return CreateTokenPrivate(JObject.Parse(payload), signingCredentials, encryptingCredentials, null, null, null);
	}

	public virtual string CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, IDictionary<string, object> additionalHeaderClaims)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (additionalHeaderClaims == null)
		{
			throw LogHelper.LogArgumentNullException("additionalHeaderClaims");
		}
		return CreateTokenPrivate(JObject.Parse(payload), signingCredentials, encryptingCredentials, null, additionalHeaderClaims, null);
	}

	public virtual string CreateToken(string payload, EncryptingCredentials encryptingCredentials, string compressionAlgorithm)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (string.IsNullOrEmpty(compressionAlgorithm))
		{
			throw LogHelper.LogArgumentNullException("compressionAlgorithm");
		}
		return CreateTokenPrivate(JObject.Parse(payload), null, encryptingCredentials, compressionAlgorithm, null, null);
	}

	public virtual string CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, string compressionAlgorithm)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (string.IsNullOrEmpty(compressionAlgorithm))
		{
			throw LogHelper.LogArgumentNullException("compressionAlgorithm");
		}
		return CreateTokenPrivate(JObject.Parse(payload), signingCredentials, encryptingCredentials, compressionAlgorithm, null, null);
	}

	public virtual string CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, string compressionAlgorithm, IDictionary<string, object> additionalHeaderClaims)
	{
		if (string.IsNullOrEmpty(payload))
		{
			throw LogHelper.LogArgumentNullException("payload");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (string.IsNullOrEmpty(compressionAlgorithm))
		{
			throw LogHelper.LogArgumentNullException("compressionAlgorithm");
		}
		if (additionalHeaderClaims == null)
		{
			throw LogHelper.LogArgumentNullException("additionalHeaderClaims");
		}
		return CreateTokenPrivate(JObject.Parse(payload), signingCredentials, encryptingCredentials, compressionAlgorithm, additionalHeaderClaims, null);
	}

	private string CreateTokenPrivate(JObject payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, string compressionAlgorithm, IDictionary<string, object> additionalHeaderClaims, string tokenType)
	{
		if (additionalHeaderClaims != null && additionalHeaderClaims.Count > 0 && additionalHeaderClaims.Keys.Intersect<string>(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant("IDX14116: ''{0}' cannot contain the following claims: '{1}'. These values are added by default (if necessary) during security token creation.", "additionalHeaderClaims", string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters))));
		}
		JObject jObject = CreateDefaultJWSHeader(signingCredentials, tokenType);
		if (encryptingCredentials == null && additionalHeaderClaims != null && additionalHeaderClaims.Count > 0)
		{
			jObject.Merge(JObject.FromObject(additionalHeaderClaims));
		}
		string text = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(jObject.ToString(Formatting.None)));
		if (base.SetDefaultTimesOnTokenCreation)
		{
			long intDate = EpochTime.GetIntDate(DateTime.UtcNow);
			if (!payload.TryGetValue("exp", out var value))
			{
				payload.Add("exp", intDate + base.TokenLifetimeInMinutes * 60);
			}
			if (!payload.TryGetValue("iat", out value))
			{
				payload.Add("iat", intDate);
			}
			if (!payload.TryGetValue("nbf", out value))
			{
				payload.Add("nbf", intDate);
			}
		}
		string text2 = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(payload.ToString(Formatting.None)));
		string text3 = text + "." + text2;
		string text4 = ((signingCredentials == null) ? string.Empty : JwtTokenUtilities.CreateEncodedSignature(text3, signingCredentials));
		if (encryptingCredentials != null)
		{
			return EncryptTokenPrivate(text3 + "." + text4, encryptingCredentials, compressionAlgorithm, additionalHeaderClaims, tokenType);
		}
		return text3 + "." + text4;
	}

	private static byte[] CompressToken(string token, string compressionAlgorithm)
	{
		if (token == null)
		{
			throw LogHelper.LogArgumentNullException("token");
		}
		if (string.IsNullOrEmpty(compressionAlgorithm))
		{
			throw LogHelper.LogArgumentNullException("compressionAlgorithm");
		}
		if (!CompressionProviderFactory.Default.IsSupportedAlgorithm(compressionAlgorithm))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10682: Compression algorithm '{0}' is not supported.", compressionAlgorithm)));
		}
		return CompressionProviderFactory.Default.CreateCompressionProvider(compressionAlgorithm).Compress(Encoding.UTF8.GetBytes(token)) ?? throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10680: Failed to compress using algorithm '{0}'.", compressionAlgorithm)));
	}

	protected virtual ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters)
	{
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		string text = jwtToken.Issuer;
		if (string.IsNullOrWhiteSpace(text))
		{
			LogHelper.LogVerbose("IDX10244: Issuer is null or empty. Using runtime default for creating claims '{0}'.", "LOCAL AUTHORITY");
			text = "LOCAL AUTHORITY";
		}
		return CreateClaimsIdentity(jwtToken, validationParameters, text);
	}

	private ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters, string actualIssuer)
	{
		ClaimsIdentity claimsIdentity = validationParameters.CreateClaimsIdentity(jwtToken, actualIssuer);
		foreach (Claim claim2 in jwtToken.Claims)
		{
			string type = claim2.Type;
			if (type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
			{
				if (claimsIdentity.Actor != null)
				{
					throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX14112: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'", "actort", claim2.Value)));
				}
				if (CanReadToken(claim2.Value))
				{
					JsonWebToken jwtToken2 = ReadToken(claim2.Value) as JsonWebToken;
					claimsIdentity.Actor = CreateClaimsIdentity(jwtToken2, validationParameters, actualIssuer);
				}
			}
			if (claim2.Properties.Count == 0)
			{
				claimsIdentity.AddClaim(new Claim(type, claim2.Value, claim2.ValueType, actualIssuer, actualIssuer, claimsIdentity));
				continue;
			}
			Claim claim = new Claim(type, claim2.Value, claim2.ValueType, actualIssuer, actualIssuer, claimsIdentity);
			foreach (KeyValuePair<string, string> property in claim2.Properties)
			{
				claim.Properties[property.Key] = property.Value;
			}
			claimsIdentity.AddClaim(claim);
		}
		return claimsIdentity;
	}

	public string DecryptToken(JsonWebToken jwtToken, TokenValidationParameters validationParameters)
	{
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (string.IsNullOrEmpty(jwtToken.Enc))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant("IDX10612: Decryption failed. Header.Enc is null or empty, it must be specified.")));
		}
		IEnumerable<SecurityKey> contentEncryptionKeys = GetContentEncryptionKeys(jwtToken, validationParameters);
		return JwtTokenUtilities.DecryptJwtToken(jwtToken, validationParameters, new JwtTokenDecryptionParameters
		{
			Alg = jwtToken.Alg,
			AuthenticationTag = jwtToken.AuthenticationTag,
			Ciphertext = jwtToken.Ciphertext,
			DecompressionFunction = JwtTokenUtilities.DecompressToken,
			Enc = jwtToken.Enc,
			EncodedHeader = jwtToken.EncodedHeader,
			EncodedToken = jwtToken.EncodedToken,
			InitializationVector = jwtToken.InitializationVector,
			Keys = contentEncryptionKeys,
			Zip = jwtToken.Zip
		});
	}

	public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials)
	{
		if (string.IsNullOrEmpty(innerJwt))
		{
			throw LogHelper.LogArgumentNullException("innerJwt");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		return EncryptTokenPrivate(innerJwt, encryptingCredentials, null, null, null);
	}

	public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials, IDictionary<string, object> additionalHeaderClaims)
	{
		if (string.IsNullOrEmpty(innerJwt))
		{
			throw LogHelper.LogArgumentNullException("innerJwt");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (additionalHeaderClaims == null)
		{
			throw LogHelper.LogArgumentNullException("additionalHeaderClaims");
		}
		return EncryptTokenPrivate(innerJwt, encryptingCredentials, null, additionalHeaderClaims, null);
	}

	public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials, string algorithm)
	{
		if (string.IsNullOrEmpty(innerJwt))
		{
			throw LogHelper.LogArgumentNullException("innerJwt");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		return EncryptTokenPrivate(innerJwt, encryptingCredentials, algorithm, null, null);
	}

	public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials, string algorithm, IDictionary<string, object> additionalHeaderClaims)
	{
		if (string.IsNullOrEmpty(innerJwt))
		{
			throw LogHelper.LogArgumentNullException("innerJwt");
		}
		if (encryptingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("encryptingCredentials");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (additionalHeaderClaims == null)
		{
			throw LogHelper.LogArgumentNullException("additionalHeaderClaims");
		}
		return EncryptTokenPrivate(innerJwt, encryptingCredentials, algorithm, additionalHeaderClaims, null);
	}

	private static string EncryptTokenPrivate(string innerJwt, EncryptingCredentials encryptingCredentials, string compressionAlgorithm, IDictionary<string, object> additionalHeaderClaims, string tokenType)
	{
		CryptoProviderFactory cryptoProviderFactory = encryptingCredentials.CryptoProviderFactory ?? encryptingCredentials.Key.CryptoProviderFactory;
		if (cryptoProviderFactory == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException("IDX10620: Unable to obtain a CryptoProviderFactory, both EncryptingCredentials.CryptoProviderFactory and EncryptingCredentials.Key.CrypoProviderFactory are null."));
		}
		byte[] wrappedKey = null;
		SecurityKey securityKey = JwtTokenUtilities.GetSecurityKey(encryptingCredentials, cryptoProviderFactory, out wrappedKey);
		using AuthenticatedEncryptionProvider authenticatedEncryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(securityKey, encryptingCredentials.Enc);
		if (authenticatedEncryptionProvider == null)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException("IDX14103: Failed to create the token encryption provider."));
		}
		JObject jObject = CreateDefaultJWEHeader(encryptingCredentials, compressionAlgorithm, tokenType);
		if (additionalHeaderClaims != null)
		{
			jObject.Merge(JObject.FromObject(additionalHeaderClaims));
		}
		byte[] plaintext;
		if (!string.IsNullOrEmpty(compressionAlgorithm))
		{
			try
			{
				plaintext = CompressToken(innerJwt, compressionAlgorithm);
			}
			catch (Exception inner)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenCompressionFailedException(LogHelper.FormatInvariant("IDX10680: Failed to compress using algorithm '{0}'.", compressionAlgorithm), inner));
			}
		}
		else
		{
			plaintext = Encoding.UTF8.GetBytes(innerJwt);
		}
		try
		{
			string text = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(jObject.ToString(Formatting.None)));
			AuthenticatedEncryptionResult authenticatedEncryptionResult = authenticatedEncryptionProvider.Encrypt(plaintext, Encoding.ASCII.GetBytes(text));
			return "dir".Equals(encryptingCredentials.Alg, StringComparison.Ordinal) ? string.Join(".", text, string.Empty, Base64UrlEncoder.Encode(authenticatedEncryptionResult.IV), Base64UrlEncoder.Encode(authenticatedEncryptionResult.Ciphertext), Base64UrlEncoder.Encode(authenticatedEncryptionResult.AuthenticationTag)) : string.Join(".", text, Base64UrlEncoder.Encode(wrappedKey), Base64UrlEncoder.Encode(authenticatedEncryptionResult.IV), Base64UrlEncoder.Encode(authenticatedEncryptionResult.Ciphertext), Base64UrlEncoder.Encode(authenticatedEncryptionResult.AuthenticationTag));
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant("IDX10616: Encryption failed. EncryptionProvider failed for: Algorithm: '{0}', SecurityKey: '{1}'. See inner exception.", encryptingCredentials.Enc, encryptingCredentials.Key), innerException));
		}
	}

	internal IEnumerable<SecurityKey> GetContentEncryptionKeys(JsonWebToken jwtToken, TokenValidationParameters validationParameters)
	{
		IEnumerable<SecurityKey> enumerable = null;
		if (validationParameters.TokenDecryptionKeyResolver != null)
		{
			enumerable = validationParameters.TokenDecryptionKeyResolver(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters);
		}
		else
		{
			SecurityKey securityKey = ResolveTokenDecryptionKey(jwtToken.EncodedToken, jwtToken, validationParameters);
			if (securityKey != null)
			{
				enumerable = new List<SecurityKey> { securityKey };
			}
		}
		if (enumerable == null)
		{
			enumerable = JwtTokenUtilities.GetAllDecryptionKeys(validationParameters);
		}
		if (jwtToken.Alg.Equals("dir", StringComparison.Ordinal))
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
				if (item.CryptoProviderFactory.IsSupportedAlgorithm(jwtToken.Alg, item))
				{
					byte[] key = item.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(item, jwtToken.Alg).UnwrapKey(Base64UrlEncoder.DecodeBytes(jwtToken.EncryptedKey));
					list.Add(new SymmetricSecurityKey(key));
				}
			}
			catch (Exception ex)
			{
				stringBuilder.AppendLine(ex.ToString());
			}
			stringBuilder2.AppendLine(item.ToString());
		}
		if (list.Count > 0 || stringBuilder.Length == 0)
		{
			return list;
		}
		throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant("IDX10618: Key unwrap failed using decryption Keys: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'.", stringBuilder2, stringBuilder, jwtToken)));
	}

	protected virtual SecurityKey ResolveTokenDecryptionKey(string token, JsonWebToken jwtToken, TokenValidationParameters validationParameters)
	{
		if (jwtToken == null)
		{
			throw LogHelper.LogArgumentNullException("jwtToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (!string.IsNullOrEmpty(jwtToken.Kid))
		{
			if (validationParameters.TokenDecryptionKey != null && string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.Kid, (validationParameters.TokenDecryptionKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				return validationParameters.TokenDecryptionKey;
			}
			if (validationParameters.TokenDecryptionKeys != null)
			{
				foreach (SecurityKey tokenDecryptionKey in validationParameters.TokenDecryptionKeys)
				{
					if (tokenDecryptionKey != null && string.Equals(tokenDecryptionKey.KeyId, jwtToken.Kid, (tokenDecryptionKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
					{
						return tokenDecryptionKey;
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(jwtToken.X5t))
		{
			if (validationParameters.TokenDecryptionKey != null)
			{
				if (string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.X5t, (validationParameters.TokenDecryptionKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
				{
					return validationParameters.TokenDecryptionKey;
				}
				if (validationParameters.TokenDecryptionKey is X509SecurityKey x509SecurityKey && string.Equals(x509SecurityKey.X5t, jwtToken.X5t, StringComparison.OrdinalIgnoreCase))
				{
					return validationParameters.TokenDecryptionKey;
				}
			}
			if (validationParameters.TokenDecryptionKeys != null)
			{
				foreach (SecurityKey tokenDecryptionKey2 in validationParameters.TokenDecryptionKeys)
				{
					if (tokenDecryptionKey2 != null && string.Equals(tokenDecryptionKey2.KeyId, jwtToken.X5t, (tokenDecryptionKey2 is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
					{
						return tokenDecryptionKey2;
					}
					if (tokenDecryptionKey2 is X509SecurityKey x509SecurityKey2 && string.Equals(x509SecurityKey2.X5t, jwtToken.X5t, StringComparison.OrdinalIgnoreCase))
					{
						return tokenDecryptionKey2;
					}
				}
			}
		}
		return null;
	}

	public virtual JsonWebToken ReadJsonWebToken(string token)
	{
		if (string.IsNullOrEmpty(token))
		{
			throw LogHelper.LogArgumentNullException("token");
		}
		if (token.Length > MaximumTokenSizeInBytes)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.", token.Length, MaximumTokenSizeInBytes)));
		}
		return new JsonWebToken(token);
	}

	public virtual SecurityToken ReadToken(string token)
	{
		return ReadJsonWebToken(token);
	}

	public virtual TokenValidationResult ValidateToken(string token, TokenValidationParameters validationParameters)
	{
		if (string.IsNullOrEmpty(token))
		{
			return new TokenValidationResult
			{
				Exception = LogHelper.LogArgumentNullException("token"),
				IsValid = false
			};
		}
		if (validationParameters == null)
		{
			return new TokenValidationResult
			{
				Exception = LogHelper.LogArgumentNullException("validationParameters"),
				IsValid = false
			};
		}
		if (token.Length > MaximumTokenSizeInBytes)
		{
			TokenValidationResult tokenValidationResult = new TokenValidationResult();
			tokenValidationResult.Exception = LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10209: Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'.", token.Length, MaximumTokenSizeInBytes)));
			tokenValidationResult.IsValid = false;
			return tokenValidationResult;
		}
		string[] array = token.Split(new char[1] { '.' }, 6);
		if (array.Length != 3 && array.Length != 5)
		{
			TokenValidationResult tokenValidationResult = new TokenValidationResult();
			tokenValidationResult.Exception = LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX14111: JWT: '{0}' must have three segments (JWS) or five segments (JWE).", token)));
			tokenValidationResult.IsValid = false;
			return tokenValidationResult;
		}
		try
		{
			if (array.Length == 5)
			{
				JsonWebToken jsonWebToken = new JsonWebToken(token);
				JsonWebToken jsonWebToken3 = (jsonWebToken.InnerToken = ValidateSignature(DecryptToken(jsonWebToken, validationParameters), validationParameters));
				TokenValidationResult tokenValidationResult2 = ValidateTokenPayload(jsonWebToken3, validationParameters);
				return new TokenValidationResult
				{
					SecurityToken = jsonWebToken,
					ClaimsIdentity = tokenValidationResult2.ClaimsIdentity,
					IsValid = true,
					TokenType = tokenValidationResult2.TokenType
				};
			}
			JsonWebToken jsonWebToken4 = ValidateSignature(token, validationParameters);
			return ValidateTokenPayload(jsonWebToken4, validationParameters);
		}
		catch (Exception exception)
		{
			return new TokenValidationResult
			{
				Exception = exception,
				IsValid = false
			};
		}
	}

	private TokenValidationResult ValidateTokenPayload(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters)
	{
		DateTime validTo = jsonWebToken.ValidTo;
		Validators.ValidateLifetime(jsonWebToken.ValidFrom, validTo, jsonWebToken, validationParameters);
		Validators.ValidateAudience(jsonWebToken.Audiences, jsonWebToken, validationParameters);
		string actualIssuer = Validators.ValidateIssuer(jsonWebToken.Issuer, jsonWebToken, validationParameters);
		Validators.ValidateTokenReplay(validTo, jsonWebToken.EncodedToken, validationParameters);
		if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jsonWebToken.Actor))
		{
			ValidateToken(jsonWebToken.Actor, validationParameters.ActorValidationParameters ?? validationParameters);
		}
		Validators.ValidateIssuerSecurityKey(jsonWebToken.SigningKey, jsonWebToken, validationParameters);
		string tokenType = Validators.ValidateTokenType(jsonWebToken.Typ, jsonWebToken, validationParameters);
		return new TokenValidationResult
		{
			SecurityToken = jsonWebToken,
			ClaimsIdentity = CreateClaimsIdentity(jsonWebToken, validationParameters, actualIssuer),
			IsValid = true,
			TokenType = tokenType
		};
	}

	private static JsonWebToken ValidateSignature(string token, TokenValidationParameters validationParameters)
	{
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
			SecurityToken securityToken = validationParameters.SignatureValidator(token, validationParameters);
			if (securityToken == null)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10505: Signature validation failed. The user defined 'Delegate' specified on TokenValidationParameters returned null when validating token: '{0}'.", token)));
			}
			if (!(securityToken is JsonWebToken result))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10506: Signature validation failed. The user defined 'Delegate' specified on TokenValidationParameters did not return a '{0}', but returned a '{1}' when validating token: '{2}'.", typeof(JsonWebToken), securityToken.GetType(), token)));
			}
			return result;
		}
		JsonWebToken jsonWebToken = null;
		if (validationParameters.TokenReader != null)
		{
			SecurityToken securityToken2 = validationParameters.TokenReader(token, validationParameters);
			if (securityToken2 == null)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10510: Signature validation failed. The user defined 'Delegate' specified in TokenValidationParameters returned null when reading token: '{0}'.", token)));
			}
			jsonWebToken = securityToken2 as JsonWebToken;
			if (jsonWebToken == null)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10509: Signature validation failed. The user defined 'Delegate' specified in TokenValidationParameters did not return a '{0}', but returned a '{1}' when reading token: '{2}'.", typeof(JsonWebToken), securityToken2.GetType(), token)));
			}
		}
		else
		{
			jsonWebToken = new JsonWebToken(token);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(jsonWebToken.EncodedHeader + "." + jsonWebToken.EncodedPayload);
		if (string.IsNullOrEmpty(jsonWebToken.EncodedSignature))
		{
			if (validationParameters.RequireSignedTokens)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10504: Unable to validate signature, token does not have a signature: '{0}'.", token)));
			}
			return jsonWebToken;
		}
		bool flag = false;
		IEnumerable<SecurityKey> enumerable = null;
		if (validationParameters.IssuerSigningKeyResolver != null)
		{
			enumerable = validationParameters.IssuerSigningKeyResolver(token, jsonWebToken, jsonWebToken.Kid, validationParameters);
		}
		else
		{
			SecurityKey securityKey = JwtTokenUtilities.ResolveTokenSigningKey(jsonWebToken.Kid, jsonWebToken.X5t, validationParameters);
			if (securityKey != null)
			{
				flag = true;
				enumerable = new List<SecurityKey> { securityKey };
			}
		}
		if (enumerable == null && validationParameters.TryAllIssuerSigningKeys)
		{
			enumerable = TokenUtilities.GetAllSigningKeys(validationParameters);
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		bool flag2 = !string.IsNullOrEmpty(jsonWebToken.Kid);
		byte[] signature;
		try
		{
			signature = Base64UrlEncoder.DecodeBytes(jsonWebToken.EncodedSignature);
		}
		catch (FormatException innerException)
		{
			throw new SecurityTokenInvalidSignatureException("IDX10508: Signature validation failed. Signature is improperly formatted.", innerException);
		}
		if (enumerable != null)
		{
			foreach (SecurityKey item in enumerable)
			{
				try
				{
					if (ValidateSignature(bytes, signature, item, jsonWebToken.Alg, jsonWebToken, validationParameters))
					{
						LogHelper.LogInformation("IDX10242: Security token: '{0}' has a valid signature.", token);
						jsonWebToken.SigningKey = item;
						return jsonWebToken;
					}
				}
				catch (Exception ex)
				{
					stringBuilder.AppendLine(ex.ToString());
				}
				if (item != null)
				{
					stringBuilder2.AppendLine(item.ToString() + " , KeyId: " + item.KeyId);
					if (flag2 && !flag && item.KeyId != null)
					{
						flag = jsonWebToken.Kid.Equals(item.KeyId, (item is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
					}
				}
			}
		}
		if (flag2)
		{
			if (flag)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant("IDX10511: Signature validation failed. Keys tried: '{0}'. \nkid: '{1}'. \nExceptions caught:\n '{2}'.\ntoken: '{3}'.", stringBuilder2, jsonWebToken.Kid, stringBuilder, jsonWebToken)));
			}
			throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant("IDX10501: Signature validation failed. Unable to match key: \nkid: '{0}'.\nExceptions caught:\n '{1}'. \ntoken: '{2}'.", jsonWebToken.Kid, stringBuilder, jsonWebToken)));
		}
		if (stringBuilder2.Length > 0)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant("IDX10503: Signature validation failed. Token does not have a kid. Keys tried: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'.", stringBuilder2, stringBuilder, jsonWebToken)));
		}
		throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException("IDX10500: Signature validation failed. No security keys were provided to validate the signature."));
	}

	internal static bool ValidateSignature(byte[] encodedBytes, byte[] signature, SecurityKey key, string algorithm, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		CryptoProviderFactory cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
		if (!cryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
		{
			LogHelper.LogInformation("IDX14000: Signing JWT is not supported for: Algorithm: '{0}', SecurityKey: '{1}'.", algorithm, key);
			return false;
		}
		Validators.ValidateAlgorithm(algorithm, key, securityToken, validationParameters);
		SignatureProvider signatureProvider = cryptoProviderFactory.CreateForVerifying(key, algorithm);
		if (signatureProvider == null)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10636: CryptoProviderFactory.CreateForVerifying returned null for key: '{0}', signatureAlgorithm: '{1}'.", (key == null) ? "Null" : key.ToString(), algorithm ?? "Null")));
		}
		try
		{
			return signatureProvider.Verify(encodedBytes, signature);
		}
		finally
		{
			cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
		}
	}
}
