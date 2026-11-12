using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class JsonWebKeyConverter
{
	public static JsonWebKey ConvertFromSecurityKey(SecurityKey key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (key is RsaSecurityKey key2)
		{
			return ConvertFromRSASecurityKey(key2);
		}
		if (key is SymmetricSecurityKey key3)
		{
			return ConvertFromSymmetricSecurityKey(key3);
		}
		if (key is X509SecurityKey key4)
		{
			return ConvertFromX509SecurityKey(key4);
		}
		throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10674: JsonWebKeyConverter does not support SecurityKey of type: {0}", key.GetType().FullName)));
	}

	public static JsonWebKey ConvertFromRSASecurityKey(RsaSecurityKey key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		RSAParameters rSAParameters;
		if (key.Rsa != null)
		{
			try
			{
				rSAParameters = key.Rsa.ExportParameters(includePrivateParameters: true);
			}
			catch
			{
				rSAParameters = key.Rsa.ExportParameters(includePrivateParameters: false);
			}
		}
		else
		{
			rSAParameters = key.Parameters;
		}
		return new JsonWebKey
		{
			N = ((rSAParameters.Modulus != null) ? Base64UrlEncoder.Encode(rSAParameters.Modulus) : null),
			E = ((rSAParameters.Exponent != null) ? Base64UrlEncoder.Encode(rSAParameters.Exponent) : null),
			D = ((rSAParameters.D != null) ? Base64UrlEncoder.Encode(rSAParameters.D) : null),
			P = ((rSAParameters.P != null) ? Base64UrlEncoder.Encode(rSAParameters.P) : null),
			Q = ((rSAParameters.Q != null) ? Base64UrlEncoder.Encode(rSAParameters.Q) : null),
			DP = ((rSAParameters.DP != null) ? Base64UrlEncoder.Encode(rSAParameters.DP) : null),
			DQ = ((rSAParameters.DQ != null) ? Base64UrlEncoder.Encode(rSAParameters.DQ) : null),
			QI = ((rSAParameters.InverseQ != null) ? Base64UrlEncoder.Encode(rSAParameters.InverseQ) : null),
			Kty = "RSA",
			Kid = key.KeyId,
			ConvertedSecurityKey = key
		};
	}

	public static JsonWebKey ConvertFromX509SecurityKey(X509SecurityKey key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		JsonWebKey jsonWebKey = new JsonWebKey
		{
			Kty = "RSA",
			Kid = key.KeyId,
			X5t = key.X5t,
			ConvertedSecurityKey = key
		};
		if (key.Certificate.RawData != null)
		{
			jsonWebKey.X5c.Add(Convert.ToBase64String(key.Certificate.RawData));
		}
		return jsonWebKey;
	}

	public static JsonWebKey ConvertFromX509SecurityKey(X509SecurityKey key, bool representAsRsaKey)
	{
		if (!representAsRsaKey)
		{
			return ConvertFromX509SecurityKey(key);
		}
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		RSA rsa = ((key.PrivateKeyStatus != 0) ? (key.PublicKey as RSA) : (key.PrivateKey as RSA));
		return ConvertFromRSASecurityKey(new RsaSecurityKey(rsa)
		{
			KeyId = key.KeyId
		});
	}

	public static JsonWebKey ConvertFromSymmetricSecurityKey(SymmetricSecurityKey key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		return new JsonWebKey
		{
			K = Base64UrlEncoder.Encode(key.Key),
			Kid = key.KeyId,
			Kty = "oct",
			ConvertedSecurityKey = key
		};
	}

	internal static bool TryConvertToSecurityKey(JsonWebKey webKey, out SecurityKey key)
	{
		if (webKey.ConvertedSecurityKey != null)
		{
			key = webKey.ConvertedSecurityKey;
			return true;
		}
		key = null;
		try
		{
			if ("RSA".Equals(webKey.Kty, StringComparison.Ordinal))
			{
				if (TryConvertToX509SecurityKey(webKey, out key))
				{
					return true;
				}
				if (TryCreateToRsaSecurityKey(webKey, out key))
				{
					return true;
				}
			}
			else
			{
				if ("EC".Equals(webKey.Kty, StringComparison.Ordinal))
				{
					return TryConvertToECDsaSecurityKey(webKey, out key);
				}
				if ("oct".Equals(webKey.Kty, StringComparison.Ordinal))
				{
					return TryConvertToSymmetricSecurityKey(webKey, out key);
				}
			}
		}
		catch (Exception ex)
		{
			LogHelper.LogWarning(LogHelper.FormatInvariant("IDX10813: Unable to create a {0} from the properties found in the JsonWebKey: '{1}', Exception '{2}'.", typeof(SecurityKey), webKey, ex));
		}
		LogHelper.LogWarning(LogHelper.FormatInvariant("IDX10812: Unable to create a {0} from the properties found in the JsonWebKey: '{1}'.", typeof(SecurityKey), webKey));
		return false;
	}

	internal static bool TryConvertToSymmetricSecurityKey(JsonWebKey webKey, out SecurityKey key)
	{
		if (webKey.ConvertedSecurityKey is SymmetricSecurityKey)
		{
			key = webKey.ConvertedSecurityKey;
			return true;
		}
		key = null;
		if (string.IsNullOrEmpty(webKey.K))
		{
			return false;
		}
		try
		{
			key = new SymmetricSecurityKey(webKey);
			return true;
		}
		catch (Exception ex)
		{
			LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10813: Unable to create a {0} from the properties found in the JsonWebKey: '{1}', Exception '{2}'.", typeof(SymmetricSecurityKey), webKey, ex), ex));
		}
		return false;
	}

	internal static bool TryConvertToX509SecurityKey(JsonWebKey webKey, out SecurityKey key)
	{
		if (webKey.ConvertedSecurityKey is X509SecurityKey)
		{
			key = webKey.ConvertedSecurityKey;
			return true;
		}
		key = null;
		if (webKey.X5c == null || webKey.X5c.Count == 0)
		{
			return false;
		}
		try
		{
			key = new X509SecurityKey(webKey);
			return true;
		}
		catch (Exception ex)
		{
			LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10813: Unable to create a {0} from the properties found in the JsonWebKey: '{1}', Exception '{2}'.", typeof(X509SecurityKey), webKey, ex), ex));
		}
		return false;
	}

	internal static bool TryCreateToRsaSecurityKey(JsonWebKey webKey, out SecurityKey key)
	{
		if (webKey.ConvertedSecurityKey is RsaSecurityKey)
		{
			key = webKey.ConvertedSecurityKey;
			return true;
		}
		key = null;
		if (string.IsNullOrWhiteSpace(webKey.E) || string.IsNullOrWhiteSpace(webKey.N))
		{
			return false;
		}
		try
		{
			key = new RsaSecurityKey(webKey);
			return true;
		}
		catch (Exception ex)
		{
			LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10813: Unable to create a {0} from the properties found in the JsonWebKey: '{1}', Exception '{2}'.", typeof(RsaSecurityKey), webKey, ex), ex));
		}
		return false;
	}

	internal static bool TryConvertToECDsaSecurityKey(JsonWebKey webKey, out SecurityKey key)
	{
		if (webKey.ConvertedSecurityKey is ECDsaSecurityKey)
		{
			key = webKey.ConvertedSecurityKey;
			return true;
		}
		key = null;
		if (string.IsNullOrEmpty(webKey.Crv) || string.IsNullOrEmpty(webKey.X) || string.IsNullOrEmpty(webKey.Y))
		{
			return false;
		}
		try
		{
			key = new ECDsaSecurityKey(webKey, !string.IsNullOrEmpty(webKey.D));
			return true;
		}
		catch (Exception ex)
		{
			LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10813: Unable to create a {0} from the properties found in the JsonWebKey: '{1}', Exception '{2}'.", typeof(ECDsaSecurityKey), webKey, ex), ex));
		}
		return false;
	}
}
