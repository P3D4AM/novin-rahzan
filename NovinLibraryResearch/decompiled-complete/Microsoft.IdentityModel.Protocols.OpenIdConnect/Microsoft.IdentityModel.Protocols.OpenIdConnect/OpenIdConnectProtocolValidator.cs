using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectProtocolValidator
{
	private IDictionary<string, string> _hashAlgorithmMap = new Dictionary<string, string>
	{
		{ "ES256", "SHA256" },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256", "SHA256" },
		{ "HS256", "SHA256" },
		{ "RS256", "SHA256" },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "SHA256" },
		{ "PS256", "SHA256" },
		{ "ES384", "SHA384" },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384", "SHA384" },
		{ "HS384", "SHA384" },
		{ "RS384", "SHA384" },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", "SHA384" },
		{ "PS384", "SHA384" },
		{ "ES512", "SHA512" },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512", "SHA512" },
		{ "HS512", "SHA512" },
		{ "RS512", "SHA512" },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", "SHA512" },
		{ "PS512", "SHA512" }
	};

	private TimeSpan _nonceLifetime = DefaultNonceLifetime;

	private CryptoProviderFactory _cryptoProviderFactory;

	public static readonly TimeSpan DefaultNonceLifetime = TimeSpan.FromMinutes(60.0);

	public IDictionary<string, string> HashAlgorithmMap => _hashAlgorithmMap;

	public TimeSpan NonceLifetime
	{
		get
		{
			return _nonceLifetime;
		}
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX21105: NonceLifetime must be greater than zero. value: '{0}'", value)));
			}
			_nonceLifetime = value;
		}
	}

	[DefaultValue(false)]
	public bool RequireAcr { get; set; }

	[DefaultValue(false)]
	public bool RequireAmr { get; set; }

	[DefaultValue(false)]
	public bool RequireAuthTime { get; set; }

	[DefaultValue(false)]
	public bool RequireAzp { get; set; }

	[DefaultValue(true)]
	public bool RequireNonce { get; set; }

	[DefaultValue(true)]
	public bool RequireState { get; set; }

	[DefaultValue(true)]
	public bool RequireStateValidation { get; set; }

	[DefaultValue(true)]
	public bool RequireSub { get; set; } = RequireSubByDefault;


	public static bool RequireSubByDefault { get; set; } = true;


	[DefaultValue(true)]
	public bool RequireTimeStampInNonce { get; set; }

	public IdTokenValidator IdTokenValidator { get; set; }

	public CryptoProviderFactory CryptoProviderFactory
	{
		get
		{
			return _cryptoProviderFactory;
		}
		set
		{
			if (value == null)
			{
				throw LogHelper.LogArgumentNullException("value");
			}
			_cryptoProviderFactory = value;
		}
	}

	public OpenIdConnectProtocolValidator()
	{
		RequireAcr = false;
		RequireAmr = false;
		RequireAuthTime = false;
		RequireAzp = false;
		RequireNonce = true;
		RequireState = true;
		RequireTimeStampInNonce = true;
		RequireStateValidation = true;
		_cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderFactory.Default);
	}

	public virtual string GenerateNonce()
	{
		LogHelper.LogVerbose("IDX21328: Generating nonce for openIdConnect message.");
		string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + Guid.NewGuid()));
		if (RequireTimeStampInNonce)
		{
			return DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture) + "." + text;
		}
		return text;
	}

	public virtual void ValidateAuthenticationResponse(OpenIdConnectProtocolValidationContext validationContext)
	{
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ProtocolMessage == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate."));
		}
		if (string.IsNullOrEmpty(validationContext.ProtocolMessage.IdToken))
		{
			if (string.IsNullOrEmpty(validationContext.ProtocolMessage.Code))
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21334: Both 'id_token' and 'code' are null in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Authorization Endpoint. Cannot process the message."));
			}
			ValidateState(validationContext);
			return;
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against."));
		}
		if (!string.IsNullOrEmpty(validationContext.ProtocolMessage.RefreshToken))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21335: 'refresh_token' cannot be present in a response message received from Authorization Endpoint."));
		}
		ValidateState(validationContext);
		ValidateIdToken(validationContext);
		ValidateNonce(validationContext);
		ValidateCHash(validationContext);
		ValidateAtHash(validationContext);
	}

	public virtual void ValidateTokenResponse(OpenIdConnectProtocolValidationContext validationContext)
	{
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ProtocolMessage == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate."));
		}
		if (string.IsNullOrEmpty(validationContext.ProtocolMessage.IdToken) || string.IsNullOrEmpty(validationContext.ProtocolMessage.AccessToken))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21336: Both 'id_token' and 'access_token' should be present in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Token Endpoint. Cannot process the message."));
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against."));
		}
		ValidateIdToken(validationContext);
		ValidateNonce(validationContext);
		if (validationContext.ValidatedIdToken.Payload.TryGetValue("at_hash", out var _))
		{
			ValidateAtHash(validationContext);
		}
	}

	public virtual void ValidateUserInfoResponse(OpenIdConnectProtocolValidationContext validationContext)
	{
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (string.IsNullOrEmpty(validationContext.UserInfoEndpointResponse))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21337: OpenIdConnectProtocolValidationContext.UserInfoEndpointResponse is null or empty, there is no OpenIdConnect Response to validate."));
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against."));
		}
		string empty = string.Empty;
		try
		{
			JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			empty = ((!jwtSecurityTokenHandler.CanReadToken(validationContext.UserInfoEndpointResponse)) ? JwtPayload.Deserialize(validationContext.UserInfoEndpointResponse).Sub : (jwtSecurityTokenHandler.ReadToken(validationContext.UserInfoEndpointResponse) as JwtSecurityToken).Payload.Sub);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21343: Unable to parse response from UserInfo endpoint: '{0}'", validationContext.UserInfoEndpointResponse), innerException));
		}
		if (string.IsNullOrEmpty(empty))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21345: OpenIdConnectProtocolValidationContext.UserInfoEndpointResponse does not contain a 'sub' claim, cannot validate."));
		}
		if (string.IsNullOrEmpty(validationContext.ValidatedIdToken.Payload.Sub))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21346: OpenIdConnectProtocolValidationContext.ValidatedIdToken does not contain a 'sub' claim, cannot validate."));
		}
		if (!string.Equals(validationContext.ValidatedIdToken.Payload.Sub, empty, StringComparison.Ordinal))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21338: Subject claim present in 'id_token': '{0}' does not match the claim received from UserInfo Endpoint: '{1}'.", validationContext.ValidatedIdToken.Payload.Sub, empty)));
		}
	}

	protected virtual void ValidateIdToken(OpenIdConnectProtocolValidationContext validationContext)
	{
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext.ValidatedIdToken");
		}
		if (IdTokenValidator != null)
		{
			try
			{
				IdTokenValidator(validationContext.ValidatedIdToken, validationContext);
				return;
			}
			catch (Exception innerException)
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21313: The id_token: '{0}' is not valid. Delegate threw exception, see inner exception for more details.", validationContext.ValidatedIdToken), innerException));
			}
		}
		JwtSecurityToken validatedIdToken = validationContext.ValidatedIdToken;
		if (validatedIdToken.Payload.Aud.Count == 0)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.", "aud".ToLowerInvariant(), validatedIdToken)));
		}
		if (!validatedIdToken.Payload.Exp.HasValue)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.", "exp".ToLowerInvariant(), validatedIdToken)));
		}
		if (!validatedIdToken.Payload.Iat.HasValue)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.", "iat".ToLowerInvariant(), validatedIdToken)));
		}
		if (validatedIdToken.Payload.Iss == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.", "iss".ToLowerInvariant(), validatedIdToken)));
		}
		if (RequireSub && string.IsNullOrWhiteSpace(validatedIdToken.Payload.Sub))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21314: OpenIdConnectProtocol requires the jwt token to have an '{0}' claim. The jwt did not contain an '{0}' claim, jwt: '{1}'.", "sub".ToLowerInvariant(), validatedIdToken)));
		}
		if (RequireAcr && string.IsNullOrWhiteSpace(validatedIdToken.Payload.Acr))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21315: RequireAcr is 'true' (default is 'false') but jwt.PayLoad.Acr is 'null or whitespace', jwt: '{0}'.", validatedIdToken)));
		}
		if (RequireAmr && validatedIdToken.Payload.Amr.Count == 0)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21316: RequireAmr is 'true' (default is 'false') but jwt.PayLoad.Amr is 'null or whitespace', jwt: '{0}'.", validatedIdToken)));
		}
		if (RequireAuthTime && !validatedIdToken.Payload.AuthTime.HasValue)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21317: RequireAuthTime is 'true' (default is 'false') but jwt.PayLoad.AuthTime is 'null or whitespace', jwt: '{0}'.", validatedIdToken)));
		}
		if (RequireAzp && string.IsNullOrWhiteSpace(validatedIdToken.Payload.Azp))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21318: RequireAzp is 'true' (default is 'false') but jwt.PayLoad.Azp is 'null or whitespace', jwt: '{0}'.", validatedIdToken)));
		}
		if (validatedIdToken.Payload.Aud.Count > 1 && string.IsNullOrEmpty(validatedIdToken.Payload.Azp))
		{
			LogHelper.LogWarning("IDX21339: The 'id_token' contains multiple audiences but 'azp' claim is missing.");
		}
		if (!string.IsNullOrEmpty(validatedIdToken.Payload.Azp))
		{
			if (string.IsNullOrEmpty(validationContext.ClientId))
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21308: 'Azp' claim exists in the 'id_token' but 'ciient_id' is null. Cannot validate the 'azp' claim."));
			}
			if (!string.Equals(validatedIdToken.Payload.Azp, validationContext.ClientId, StringComparison.Ordinal))
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21340: The 'id_token' contains 'azp' claim but its value is not equal to Client Id. 'azp': '{0}'. clientId: '{1}'.", validatedIdToken.Payload.Azp, validationContext.ClientId)));
			}
		}
	}

	public virtual HashAlgorithm GetHashAlgorithm(string algorithm)
	{
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21350: The algorithm specified in the jwt header is null or empty."));
		}
		try
		{
			if (!HashAlgorithmMap.TryGetValue(algorithm, out var value))
			{
				value = algorithm;
			}
			return CryptoProviderFactory.CreateHashAlgorithm(value);
		}
		catch (Exception innerException)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21301: The algorithm: '{0}' specified in the jwt header could not be used to create a '{1}'. See inner exception for details.", algorithm, typeof(HashAlgorithm)), innerException));
		}
	}

	private void ValidateHash(string expectedValue, string hashItem, string algorithm)
	{
		LogHelper.LogInformation("IDX21303: Validating hash of OIDC protocol message. Expected: '{0}'.", expectedValue);
		HashAlgorithm hashAlgorithm = null;
		try
		{
			hashAlgorithm = GetHashAlgorithm(algorithm);
			CheckHash(hashAlgorithm, expectedValue, hashItem, algorithm);
		}
		finally
		{
			CryptoProviderFactory.ReleaseHashAlgorithm(hashAlgorithm);
		}
	}

	private static void CheckHash(HashAlgorithm hashAlgorithm, string expectedValue, string hashItem, string algorithm)
	{
		byte[] array = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(hashItem));
		string b = Base64UrlEncoder.Encode(array, 0, array.Length / 2);
		if (!string.Equals(expectedValue, b, StringComparison.Ordinal))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException(LogHelper.FormatInvariant("IDX21300: The hash claim: '{0}' in the id_token did not validate with against: '{1}', algorithm: '{2}'.", expectedValue, hashItem, algorithm)));
		}
	}

	protected virtual void ValidateCHash(OpenIdConnectProtocolValidationContext validationContext)
	{
		LogHelper.LogVerbose("IDX21304: Validating 'c_hash' using id_token and code.");
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogArgumentNullException("ValidatedIdToken");
		}
		if (validationContext.ProtocolMessage == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate."));
		}
		if (string.IsNullOrEmpty(validationContext.ProtocolMessage.Code))
		{
			LogHelper.LogInformation("IDX21305: OpenIdConnectProtocolValidationContext.ProtocolMessage.Code is null, there is no 'code' in the OpenIdConnect Response to validate.");
			return;
		}
		if (!validationContext.ValidatedIdToken.Payload.TryGetValue("c_hash", out var value))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException(LogHelper.FormatInvariant("IDX21307: The 'c_hash' claim was not found in the id_token, but a 'code' was in the OpenIdConnectMessage, id_token: '{0}'", validationContext.ValidatedIdToken)));
		}
		if (!(value is string expectedValue))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException(LogHelper.FormatInvariant("IDX21306: The 'c_hash' claim was not a string in the 'id_token', but a 'code' was in the OpenIdConnectMessage, 'id_token': '{0}'.", validationContext.ValidatedIdToken)));
		}
		try
		{
			ValidateHash(expectedValue, validationContext.ProtocolMessage.Code, validationContext.ValidatedIdToken.Header.Alg);
		}
		catch (OpenIdConnectProtocolException innerException)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidCHashException("IDX21347: Validating the 'c_hash' failed, see inner exception.", innerException));
		}
	}

	protected virtual void ValidateAtHash(OpenIdConnectProtocolValidationContext validationContext)
	{
		LogHelper.LogVerbose("IDX21309: Validating 'at_hash' using id_token and access_token.");
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext.ValidatedIdToken");
		}
		if (validationContext.ProtocolMessage == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate."));
		}
		if (string.IsNullOrEmpty(validationContext.ProtocolMessage.AccessToken))
		{
			LogHelper.LogInformation("IDX21310: OpenIdConnectProtocolValidationContext.ProtocolMessage.AccessToken is null, there is no 'token' in the OpenIdConnect Response to validate.");
			return;
		}
		if (!validationContext.ValidatedIdToken.Payload.TryGetValue("at_hash", out var value))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException(LogHelper.FormatInvariant("IDX21312: The 'at_hash' claim was not found in the 'id_token', but a 'access_token' was in the OpenIdConnectMessage, 'id_token': '{0}'.", validationContext.ValidatedIdToken)));
		}
		if (!(value is string expectedValue))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException(LogHelper.FormatInvariant("IDX21311: The 'at_hash' claim was not a string in the 'id_token', but an 'access_token' was in the OpenIdConnectMessage, 'id_token': '{0}'.", validationContext.ValidatedIdToken)));
		}
		try
		{
			ValidateHash(expectedValue, validationContext.ProtocolMessage.AccessToken, validationContext.ValidatedIdToken.Header.Alg);
		}
		catch (OpenIdConnectProtocolException innerException)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidAtHashException("IDX21348: Validating the 'at_hash' failed, see inner exception.", innerException));
		}
	}

	protected virtual void ValidateNonce(OpenIdConnectProtocolValidationContext validationContext)
	{
		LogHelper.LogVerbose("IDX21319: Validating the nonce claim found in the id_token.");
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ValidatedIdToken == null)
		{
			throw LogHelper.LogArgumentNullException("ValidatedIdToken");
		}
		string nonce = validationContext.ValidatedIdToken.Payload.Nonce;
		if (!RequireNonce && string.IsNullOrEmpty(validationContext.Nonce) && string.IsNullOrEmpty(nonce))
		{
			LogHelper.LogInformation("IDX21322: RequireNonce is false, validationContext.Nonce is null and there is no 'nonce' in the OpenIdConnect Response to validate.");
			return;
		}
		if (string.IsNullOrEmpty(validationContext.Nonce) && string.IsNullOrEmpty(nonce))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21320: RequireNonce is '{0}'. OpenIdConnectProtocolValidationContext.Nonce and OpenIdConnectProtocol.ValidatedIdToken.Nonce are both null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.", RequireNonce)));
		}
		if (string.IsNullOrEmpty(validationContext.Nonce))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21323: RequireNonce is '{0}'. OpenIdConnectProtocolValidationContext.Nonce was null, OpenIdConnectProtocol.ValidatedIdToken.Payload.Nonce was not null. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'. Note if a 'nonce' is found it will be evaluated.", RequireNonce)));
		}
		if (string.IsNullOrEmpty(nonce))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21349: RequireNonce is '{0}'. OpenIdConnectProtocolValidationContext.Nonce was not null, OpenIdConnectProtocol.ValidatedIdToken.Payload.Nonce was null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'. Note if a 'nonce' is found it will be evaluated.", RequireNonce)));
		}
		if (!string.Equals(nonce, validationContext.Nonce, StringComparison.Ordinal))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21321: The 'nonce' found in the jwt token did not match the expected nonce.\nexpected: '{0}'\nfound in jwt: '{1}'.\njwt: '{2}'.", validationContext.Nonce, nonce, validationContext.ValidatedIdToken)));
		}
		if (RequireTimeStampInNonce)
		{
			int num = nonce.IndexOf('.');
			if (num == -1)
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21325: The 'nonce' did not contain a timestamp: '{0}'.\nFormat expected is: <epochtime>.<noncedata>.", nonce)));
			}
			string text = nonce.Substring(0, num);
			DateTime dateTime = new DateTime(1979, 1, 1);
			long num2 = -1L;
			try
			{
				num2 = Convert.ToInt64(text, CultureInfo.InvariantCulture);
			}
			catch (Exception innerException)
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21326: The 'nonce' timestamp could not be converted to a positive integer (greater than 0).\ntimestamp: '{0}'\nnonce: '{1}'.", text, nonce), innerException));
			}
			if (num2 <= 0)
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21326: The 'nonce' timestamp could not be converted to a positive integer (greater than 0).\ntimestamp: '{0}'\nnonce: '{1}'.", text, nonce)));
			}
			try
			{
				dateTime = DateTime.FromBinary(num2);
			}
			catch (Exception innerException2)
			{
				object[] obj = new object[3] { text, null, null };
				DateTime minValue = DateTime.MinValue;
				obj[1] = minValue.Ticks.ToString(CultureInfo.InvariantCulture);
				minValue = DateTime.MaxValue;
				obj[2] = minValue.Ticks.ToString(CultureInfo.InvariantCulture);
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21327: The 'nonce' timestamp: '{0}', could not be converted to a DateTime using DateTime.FromBinary({0}).\nThe value must be between: '{1}' and '{2}'.", obj), innerException2));
			}
			DateTime utcNow = DateTime.UtcNow;
			if (dateTime + NonceLifetime < utcNow)
			{
				throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidNonceException(LogHelper.FormatInvariant("IDX21324: The 'nonce' has expired: '{0}'. Time from 'nonce': '{1}', Current Time: '{2}'. NonceLifetime is: '{3}'.", nonce, dateTime.ToString(CultureInfo.InvariantCulture), utcNow.ToString(CultureInfo.InvariantCulture), NonceLifetime.ToString("c", CultureInfo.InvariantCulture))));
			}
		}
	}

	protected virtual void ValidateState(OpenIdConnectProtocolValidationContext validationContext)
	{
		if (!RequireStateValidation)
		{
			LogHelper.LogVerbose("IDX21342: 'RequireStateValidation' = false, not validating the state.");
			return;
		}
		if (validationContext == null)
		{
			throw LogHelper.LogArgumentNullException("validationContext");
		}
		if (validationContext.ProtocolMessage == null)
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolException("IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate."));
		}
		if (!RequireState && string.IsNullOrEmpty(validationContext.State) && string.IsNullOrEmpty(validationContext.ProtocolMessage.State))
		{
			LogHelper.LogInformation("IDX21341: 'RequireState' = false, OpenIdConnectProtocolValidationContext.State is null and there is no 'state' in the OpenIdConnect response to validate.");
			return;
		}
		if (string.IsNullOrEmpty(validationContext.State))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant("IDX21329: RequireState is '{0}' but the OpenIdConnectProtocolValidationContext.State is null. State cannot be validated.", RequireState)));
		}
		if (string.IsNullOrEmpty(validationContext.ProtocolMessage.State))
		{
			throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant("IDX21330: RequireState is '{0}', the OpenIdConnect Request contained 'state', but the Response does not contain 'state'.", RequireState.ToString())));
		}
		if (string.Equals(validationContext.State, validationContext.ProtocolMessage.State, StringComparison.Ordinal))
		{
			return;
		}
		throw LogHelper.LogExceptionMessage(new OpenIdConnectProtocolInvalidStateException(LogHelper.FormatInvariant("IDX21331: The 'state' parameter in the message: '{0}', does not equal the 'state' in the context: '{1}'.", validationContext.State, validationContext.ProtocolMessage.State)));
	}
}
