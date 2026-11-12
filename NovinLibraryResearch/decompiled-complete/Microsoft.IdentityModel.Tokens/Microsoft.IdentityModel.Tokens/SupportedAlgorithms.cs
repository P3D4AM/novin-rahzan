using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

internal static class SupportedAlgorithms
{
	internal static readonly ICollection<string> EcdsaSigningAlgorithms = new Collection<string> { "ES256", "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256", "ES384", "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384", "ES512", "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512" };

	internal static readonly ICollection<string> HashAlgorithms = new Collection<string> { "SHA256", "http://www.w3.org/2001/04/xmlenc#sha256", "SHA384", "http://www.w3.org/2001/04/xmldsig-more#sha384", "SHA512", "http://www.w3.org/2001/04/xmlenc#sha512" };

	internal static readonly ICollection<string> RsaEncryptionAlgorithms = new Collection<string> { "RSA-OAEP", "RSA1_5", "http://www.w3.org/2001/04/xmlenc#rsa-oaep" };

	internal static readonly ICollection<string> RsaSigningAlgorithms = new Collection<string> { "RS256", "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "RS384", "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", "RS512", "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512" };

	internal static readonly ICollection<string> RsaPssSigningAlgorithms = new Collection<string> { "PS256", "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1", "PS384", "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1", "PS512", "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1" };

	internal static readonly ICollection<string> SymmetricEncryptionAlgorithms = new Collection<string> { "A128CBC-HS256", "A192CBC-HS384", "A256CBC-HS512" };

	internal static readonly ICollection<string> SymmetricKeyWrapAlgorithms = new Collection<string> { "A128KW", "http://www.w3.org/2001/04/xmlenc#kw-aes128", "A256KW", "http://www.w3.org/2001/04/xmlenc#kw-aes256" };

	internal static readonly ICollection<string> SymmetricSigningAlgorithms = new Collection<string> { "HS256", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "HS384", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384", "HS512", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512" };

	internal static HashAlgorithmName GetHashAlgorithmName(string algorithm)
	{
		if (string.IsNullOrWhiteSpace(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		switch (algorithm)
		{
		case "ES256":
		case "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256":
		case "RS256":
		case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
		case "PS256":
		case "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1":
			return HashAlgorithmName.SHA256;
		case "ES384":
		case "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384":
		case "RS384":
		case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384":
		case "PS384":
		case "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1":
			return HashAlgorithmName.SHA384;
		case "ES512":
		case "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512":
		case "RS512":
		case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512":
		case "PS512":
		case "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1":
			return HashAlgorithmName.SHA512;
		default:
			throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("algorithm", LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm)));
		}
	}

	internal static string GetDigestFromSignatureAlgorithm(string algorithm)
	{
		if (string.IsNullOrWhiteSpace(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		switch (algorithm)
		{
		case "ES256":
		case "HS256":
		case "RS256":
		case "PS256":
			return "SHA256";
		case "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256":
		case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256":
		case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
		case "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1":
			return "http://www.w3.org/2001/04/xmlenc#sha256";
		case "ES384":
		case "HS384":
		case "RS384":
		case "PS384":
			return "SHA384";
		case "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384":
		case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384":
		case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384":
		case "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1":
			return "http://www.w3.org/2001/04/xmldsig-more#sha384";
		case "ES512":
		case "HS512":
		case "RS512":
		case "PS512":
			return "SHA512";
		case "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512":
		case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512":
		case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512":
		case "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1":
			return "http://www.w3.org/2001/04/xmlenc#sha512";
		default:
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm), "algorithm"));
		}
	}

	public static bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
	{
		if (key is RsaSecurityKey)
		{
			return IsSupportedRsaAlgorithm(algorithm, key);
		}
		if (key is X509SecurityKey x509SecurityKey)
		{
			if (!(x509SecurityKey.PublicKey is RSA))
			{
				return false;
			}
			return IsSupportedRsaAlgorithm(algorithm, key);
		}
		if (key is JsonWebKey jsonWebKey)
		{
			if ("RSA".Equals(jsonWebKey.Kty, StringComparison.Ordinal))
			{
				return IsSupportedRsaAlgorithm(algorithm, key);
			}
			if ("EC".Equals(jsonWebKey.Kty, StringComparison.Ordinal))
			{
				return IsSupportedEcdsaAlgorithm(algorithm);
			}
			if ("oct".Equals(jsonWebKey.Kty, StringComparison.Ordinal))
			{
				return IsSupportedSymmetricAlgorithm(algorithm);
			}
			return false;
		}
		if (key is ECDsaSecurityKey)
		{
			return IsSupportedEcdsaAlgorithm(algorithm);
		}
		if (key is SymmetricSecurityKey)
		{
			return IsSupportedSymmetricAlgorithm(algorithm);
		}
		return false;
	}

	internal static bool IsSupportedAuthenticatedEncryptionAlgorithm(string algorithm, SecurityKey key)
	{
		if (key == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			return false;
		}
		if (!algorithm.Equals("A128CBC-HS256", StringComparison.Ordinal) && !algorithm.Equals("A192CBC-HS384", StringComparison.Ordinal) && !algorithm.Equals("A256CBC-HS512", StringComparison.Ordinal))
		{
			return false;
		}
		if (key is SymmetricSecurityKey)
		{
			return true;
		}
		if (key is JsonWebKey jsonWebKey)
		{
			if (jsonWebKey.K != null)
			{
				return jsonWebKey.Kty == "oct";
			}
			return false;
		}
		return false;
	}

	private static bool IsSupportedEcdsaAlgorithm(string algorithm)
	{
		return EcdsaSigningAlgorithms.Contains(algorithm);
	}

	internal static bool IsSupportedHashAlgorithm(string algorithm)
	{
		return HashAlgorithms.Contains(algorithm);
	}

	internal static bool IsSupportedRsaKeyWrap(string algorithm, SecurityKey key)
	{
		if (key == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			return false;
		}
		if (!RsaEncryptionAlgorithms.Contains(algorithm))
		{
			return false;
		}
		if (key is RsaSecurityKey || key is X509SecurityKey || key is JsonWebKey { Kty: "RSA" })
		{
			return key.KeySize >= 2048;
		}
		return false;
	}

	internal static bool IsSupportedSymmetricKeyWrap(string algorithm, SecurityKey key)
	{
		if (key == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			return false;
		}
		if (!SymmetricKeyWrapAlgorithms.Contains(algorithm))
		{
			return false;
		}
		if (!(key is SymmetricSecurityKey))
		{
			if (key is JsonWebKey jsonWebKey)
			{
				return jsonWebKey.Kty == "oct";
			}
			return false;
		}
		return true;
	}

	internal static bool IsSupportedRsaAlgorithm(string algorithm, SecurityKey key)
	{
		if (!RsaSigningAlgorithms.Contains(algorithm) && !RsaEncryptionAlgorithms.Contains(algorithm))
		{
			if (RsaPssSigningAlgorithms.Contains(algorithm))
			{
				return IsSupportedRsaPss(key);
			}
			return false;
		}
		return true;
	}

	private static bool IsSupportedRsaPss(SecurityKey key)
	{
		if (key is RsaSecurityKey rsaSecurityKey && rsaSecurityKey.Rsa is RSACryptoServiceProvider)
		{
			LogHelper.LogInformation("IDX10693: RSACryptoServiceProvider doesn't support the RSASSA-PSS signature algorithm. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms");
			return false;
		}
		if (key is X509SecurityKey x509SecurityKey && x509SecurityKey.PublicKey is RSACryptoServiceProvider)
		{
			LogHelper.LogInformation("IDX10693: RSACryptoServiceProvider doesn't support the RSASSA-PSS signature algorithm. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms");
			return false;
		}
		return true;
	}

	internal static bool IsSupportedSymmetricAlgorithm(string algorithm)
	{
		if (!SymmetricEncryptionAlgorithms.Contains(algorithm) && !SymmetricKeyWrapAlgorithms.Contains(algorithm))
		{
			return SymmetricSigningAlgorithms.Contains(algorithm);
		}
		return true;
	}
}
