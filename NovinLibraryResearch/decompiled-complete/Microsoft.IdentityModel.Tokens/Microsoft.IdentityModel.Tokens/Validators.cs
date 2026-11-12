using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public static class Validators
{
	public static void ValidateAlgorithm(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.AlgorithmValidator != null)
		{
			if (!validationParameters.AlgorithmValidator(algorithm, securityKey, securityToken, validationParameters))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAlgorithmException(LogHelper.FormatInvariant("IDX10697: The user defined 'Delegate' AlgorithmValidator specified on TokenValidationParameters returned false when validating Algorithm: '{0}', SecurityKey: '{1}'.", algorithm, securityKey))
				{
					InvalidAlgorithm = algorithm
				});
			}
		}
		else if (validationParameters.ValidAlgorithms != null && validationParameters.ValidAlgorithms.Any() && !validationParameters.ValidAlgorithms.Contains<string>(algorithm, StringComparer.Ordinal))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAlgorithmException(LogHelper.FormatInvariant("IDX10696: The algorithm '{0}' is not in the user-defined accepted list of algorithms.", algorithm))
			{
				InvalidAlgorithm = algorithm
			});
		}
	}

	public static void ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.AudienceValidator != null)
		{
			if (!validationParameters.AudienceValidator(audiences, securityToken, validationParameters))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException(LogHelper.FormatInvariant("IDX10231: Audience validation failed. Delegate returned false, securitytoken: '{0}'.", securityToken))
				{
					InvalidAudience = Utility.SerializeAsSingleCommaDelimitedString(audiences)
				});
			}
			return;
		}
		if (!validationParameters.ValidateAudience)
		{
			LogHelper.LogWarning("IDX10233: ValidateAudience property on ValidationParameters is set to false. Exiting without validating the audience.");
			return;
		}
		if (audiences == null)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException("IDX10207: Unable to validate audience. The 'audiences' parameter is null.")
			{
				InvalidAudience = null
			});
		}
		if (string.IsNullOrWhiteSpace(validationParameters.ValidAudience) && validationParameters.ValidAudiences == null)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException("IDX10208: Unable to validate audience. validationParameters.ValidAudience is null or whitespace and validationParameters.ValidAudiences is null.")
			{
				InvalidAudience = Utility.SerializeAsSingleCommaDelimitedString(audiences)
			});
		}
		if (!audiences.Any())
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException(LogHelper.FormatInvariant("IDX10206: Unable to validate audience. The 'audiences' parameter is empty."))
			{
				InvalidAudience = Utility.SerializeAsSingleCommaDelimitedString(audiences)
			});
		}
		IEnumerable<string> validationParametersAudiences = ((validationParameters.ValidAudiences == null) ? new string[1] { validationParameters.ValidAudience } : ((!string.IsNullOrWhiteSpace(validationParameters.ValidAudience)) ? validationParameters.ValidAudiences.Concat(new string[1] { validationParameters.ValidAudience }) : validationParameters.ValidAudiences));
		if (AudienceIsValid(audiences, validationParameters, validationParametersAudiences))
		{
			return;
		}
		throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException(LogHelper.FormatInvariant("IDX10214: Audience validation failed. Audiences: '{0}'. Did not match: validationParameters.ValidAudience: '{1}' or validationParameters.ValidAudiences: '{2}'.", Utility.SerializeAsSingleCommaDelimitedString(audiences), validationParameters.ValidAudience ?? "null", Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidAudiences)))
		{
			InvalidAudience = Utility.SerializeAsSingleCommaDelimitedString(audiences)
		});
	}

	private static bool AudienceIsValid(IEnumerable<string> audiences, TokenValidationParameters validationParameters, IEnumerable<string> validationParametersAudiences)
	{
		foreach (string audience in audiences)
		{
			if (string.IsNullOrWhiteSpace(audience))
			{
				continue;
			}
			foreach (string validationParametersAudience in validationParametersAudiences)
			{
				if (!string.IsNullOrWhiteSpace(validationParametersAudience) && AudiencesMatch(validationParameters, audience, validationParametersAudience))
				{
					LogHelper.LogInformation("IDX10234: Audience Validated.Audience: '{0}'", audience);
					return true;
				}
			}
		}
		return false;
	}

	private static bool AudiencesMatch(TokenValidationParameters validationParameters, string tokenAudience, string validAudience)
	{
		if (validAudience.Length == tokenAudience.Length)
		{
			if (string.Equals(validAudience, tokenAudience, StringComparison.Ordinal))
			{
				return true;
			}
		}
		else if (validationParameters.IgnoreTrailingSlashWhenValidatingAudience && AudiencesMatchIgnoringTrailingSlash(tokenAudience, validAudience))
		{
			return true;
		}
		return false;
	}

	private static bool AudiencesMatchIgnoringTrailingSlash(string tokenAudience, string validAudience)
	{
		int num = -1;
		if (validAudience.Length == tokenAudience.Length + 1 && validAudience.EndsWith("/", StringComparison.InvariantCulture))
		{
			num = validAudience.Length - 1;
		}
		else if (tokenAudience.Length == validAudience.Length + 1 && tokenAudience.EndsWith("/", StringComparison.InvariantCulture))
		{
			num = tokenAudience.Length - 1;
		}
		if (num == -1)
		{
			return false;
		}
		if (string.CompareOrdinal(validAudience, 0, tokenAudience, 0, num) == 0)
		{
			LogHelper.LogInformation("IDX10234: Audience Validated.Audience: '{0}'", tokenAudience);
			return true;
		}
		return false;
	}

	public static string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.IssuerValidator != null)
		{
			return validationParameters.IssuerValidator(issuer, securityToken, validationParameters);
		}
		if (!validationParameters.ValidateIssuer)
		{
			LogHelper.LogInformation("IDX10235: ValidateIssuer property on ValidationParameters is set to false. Exiting without validating the issuer.");
			return issuer;
		}
		if (string.IsNullOrWhiteSpace(issuer))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException("IDX10211: Unable to validate issuer. The 'issuer' parameter is null or whitespace")
			{
				InvalidIssuer = issuer
			});
		}
		if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer) && validationParameters.ValidIssuers == null)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException("IDX10204: Unable to validate issuer. validationParameters.ValidIssuer is null or whitespace AND validationParameters.ValidIssuers is null.")
			{
				InvalidIssuer = issuer
			});
		}
		if (string.Equals(validationParameters.ValidIssuer, issuer, StringComparison.Ordinal))
		{
			LogHelper.LogInformation("IDX10236: Issuer Validated.Issuer: '{0}'", issuer);
			return issuer;
		}
		if (validationParameters.ValidIssuers != null)
		{
			foreach (string validIssuer in validationParameters.ValidIssuers)
			{
				if (string.IsNullOrEmpty(validIssuer))
				{
					LogHelper.LogInformation("IDX10235: ValidateIssuer property on ValidationParameters is set to false. Exiting without validating the issuer.");
				}
				else if (string.Equals(validIssuer, issuer, StringComparison.Ordinal))
				{
					LogHelper.LogInformation("IDX10236: Issuer Validated.Issuer: '{0}'", issuer);
					return issuer;
				}
			}
		}
		throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidIssuerException(LogHelper.FormatInvariant("IDX10205: Issuer validation failed. Issuer: '{0}'. Did not match: validationParameters.ValidIssuer: '{1}' or validationParameters.ValidIssuers: '{2}'.", issuer, validationParameters.ValidIssuer ?? "null", Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)))
		{
			InvalidIssuer = issuer
		});
	}

	public static void ValidateIssuerSecurityKey(SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.IssuerSigningKeyValidator != null && !validationParameters.IssuerSigningKeyValidator(securityKey, securityToken, validationParameters))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant("IDX10232: IssuerSigningKey validation failed. Delegate returned false, securityKey: '{0}'.", securityKey))
			{
				SigningKey = securityKey
			});
		}
		if (!validationParameters.ValidateIssuerSigningKey)
		{
			LogHelper.LogInformation("IDX10237: ValidateIssuerSigningKey property on ValidationParameters is set to false. Exiting without validating the issuer signing key.");
			return;
		}
		if (!validationParameters.RequireSignedTokens && securityKey == null)
		{
			LogHelper.LogInformation("IDX10252: RequireSignedTokens property on ValidationParameters is set to false and the issuer signing key is null. Exiting without validating the issuer signing key.");
			return;
		}
		if (securityKey == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentNullException("securityKey", "IDX10253: RequireSignedTokens property on ValidationParameters is set to true, but the issuer signing key is null."));
		}
		if (securityToken == null)
		{
			throw LogHelper.LogArgumentNullException("securityToken");
		}
		X509Certificate2 x509Certificate = (securityKey as X509SecurityKey)?.Certificate;
		if (x509Certificate != null)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = x509Certificate.NotBefore.ToUniversalTime();
			DateTime dateTime2 = x509Certificate.NotAfter.ToUniversalTime();
			if (dateTime > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant("IDX10248: X509SecurityKey validation failed. The associated certificate is not yet valid. ValidFrom (UTC): '{0}', Current time (UTC): '{1}'.", dateTime, utcNow)));
			}
			LogHelper.LogInformation("IDX10250: The associated certificate is valid. ValidFrom (UTC): '{0}', Current time (UTC): '{1}'.", dateTime, utcNow);
			if (dateTime2 < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate()))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSigningKeyException(LogHelper.FormatInvariant("IDX10249: X509SecurityKey validation failed. The associated certificate has expired. ValidTo (UTC): '{0}', Current time (UTC): '{1}'.", dateTime2, utcNow)));
			}
			LogHelper.LogInformation("IDX10251: The associated certificate is valid. ValidTo (UTC): '{0}', Current time (UTC): '{1}'.", dateTime2, utcNow);
		}
	}

	public static void ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.LifetimeValidator != null)
		{
			if (!validationParameters.LifetimeValidator(notBefore, expires, securityToken, validationParameters))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidLifetimeException(LogHelper.FormatInvariant("IDX10230: Lifetime validation failed. Delegate returned false, securitytoken: '{0}'.", securityToken))
				{
					NotBefore = notBefore,
					Expires = expires
				});
			}
			return;
		}
		if (!validationParameters.ValidateLifetime)
		{
			LogHelper.LogInformation("IDX10238: ValidateLifetime property on ValidationParameters is set to false. Exiting without validating the lifetime.");
			return;
		}
		if (!expires.HasValue && validationParameters.RequireExpirationTime)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenNoExpirationException(LogHelper.FormatInvariant("IDX10225: Lifetime validation failed. The token is missing an Expiration Time. Tokentype: '{0}'.", (securityToken == null) ? "null" : securityToken.GetType().ToString())));
		}
		if (notBefore.HasValue && expires.HasValue && notBefore.Value > expires.Value)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidLifetimeException(LogHelper.FormatInvariant("IDX10224: Lifetime validation failed. The NotBefore: '{0}' is after Expires: '{1}'.", notBefore.Value, expires.Value))
			{
				NotBefore = notBefore,
				Expires = expires
			});
		}
		DateTime utcNow = DateTime.UtcNow;
		if (notBefore.HasValue && notBefore.Value > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenNotYetValidException(LogHelper.FormatInvariant("IDX10222: Lifetime validation failed. The token is not yet valid. ValidFrom: '{0}', Current time: '{1}'.", notBefore.Value, utcNow))
			{
				NotBefore = notBefore.Value
			});
		}
		if (expires.HasValue && expires.Value < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate()))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenExpiredException(LogHelper.FormatInvariant("IDX10223: Lifetime validation failed. The token is expired. ValidTo: '{0}', Current time: '{1}'.", expires.Value, utcNow))
			{
				Expires = expires.Value
			});
		}
		LogHelper.LogInformation("IDX10239: Lifetime of the token is valid.");
	}

	public static void ValidateTokenReplay(DateTime? expirationTime, string securityToken, TokenValidationParameters validationParameters)
	{
		if (string.IsNullOrWhiteSpace(securityToken))
		{
			throw LogHelper.LogArgumentNullException("securityToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.TokenReplayValidator != null)
		{
			if (!validationParameters.TokenReplayValidator(expirationTime, securityToken, validationParameters))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenReplayDetectedException(LogHelper.FormatInvariant("IDX10228: The securityToken has previously been validated, securityToken: '{0}'.", securityToken)));
			}
			return;
		}
		if (!validationParameters.ValidateTokenReplay)
		{
			LogHelper.LogInformation("IDX10246: ValidateTokenReplay property on ValidationParameters is set to false. Exiting without validating the token replay.");
			return;
		}
		if (validationParameters.TokenReplayCache != null)
		{
			if (!expirationTime.HasValue)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenNoExpirationException(LogHelper.FormatInvariant("IDX10227: TokenValidationParameters.TokenReplayCache is not null, indicating to check for token replay but the security token has no expiration time: token '{0}'.", securityToken)));
			}
			if (validationParameters.TokenReplayCache.TryFind(securityToken))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenReplayDetectedException(LogHelper.FormatInvariant("IDX10228: The securityToken has previously been validated, securityToken: '{0}'.", securityToken)));
			}
			if (!validationParameters.TokenReplayCache.TryAdd(securityToken, expirationTime.Value))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenReplayAddFailedException(LogHelper.FormatInvariant("IDX10229: TokenValidationParameters.TokenReplayCache was unable to add the securityToken: '{0}'.", securityToken)));
			}
		}
		LogHelper.LogInformation("IDX10240: No token replay is detected.");
	}

	public static void ValidateTokenReplay(string securityToken, DateTime? expirationTime, TokenValidationParameters validationParameters)
	{
		ValidateTokenReplay(expirationTime, securityToken, validationParameters);
	}

	public static string ValidateTokenType(string type, SecurityToken securityToken, TokenValidationParameters validationParameters)
	{
		if (securityToken == null)
		{
			throw new ArgumentNullException("securityToken");
		}
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (validationParameters.TypeValidator == null && (validationParameters.ValidTypes == null || !validationParameters.ValidTypes.Any()))
		{
			LogHelper.LogInformation("IDX10255: TypeValidator property on ValidationParameters is null and ValidTypes is either null or empty. Exiting without validating the token type.");
			return type;
		}
		if (validationParameters.TypeValidator != null)
		{
			return validationParameters.TypeValidator(type, securityToken, validationParameters);
		}
		if (string.IsNullOrEmpty(type))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidTypeException("IDX10256: Unable to validate the token type. TokenValidationParameters.ValidTypes is set, but the 'typ' header claim is null or empty.")
			{
				InvalidType = null
			});
		}
		if (!validationParameters.ValidTypes.Contains<string>(type, StringComparer.Ordinal))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidTypeException(LogHelper.FormatInvariant("IDX10257: Token type validation failed. Type: '{0}'. Did not match: validationParameters.TokenTypes: '{1}'.", type, Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidTypes)))
			{
				InvalidType = type
			});
		}
		LogHelper.LogInformation("IDX10258: Token type validated. Type: '{0}'.", type);
		return type;
	}
}
