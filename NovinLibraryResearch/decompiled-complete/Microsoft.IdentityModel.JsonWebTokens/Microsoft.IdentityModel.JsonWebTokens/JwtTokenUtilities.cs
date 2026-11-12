using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens;

public class JwtTokenUtilities
{
	public static Regex RegexJws = new Regex("^[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]*$", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100.0));

	public static Regex RegexJwe = new Regex("^[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]*\\.[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]+\\.[A-Za-z0-9-_]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100.0));

	internal static IList<string> DefaultHeaderParameters = new List<string> { "alg", "kid", "x5t", "enc", "zip" };

	public static string CreateEncodedSignature(string input, SigningCredentials signingCredentials)
	{
		if (input == null)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		CryptoProviderFactory cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
		SignatureProvider signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm);
		if (signatureProvider == null)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10637: CryptoProviderFactory.CreateForSigning returned null for key: '{0}', signatureAlgorithm: '{1}'.", (signingCredentials.Key == null) ? "Null" : signingCredentials.Key.ToString(), signingCredentials.Algorithm ?? "Null")));
		}
		try
		{
			LogHelper.LogVerbose("IDX14200: Creating raw signature using the signature credentials.");
			return Base64UrlEncoder.Encode(signatureProvider.Sign(Encoding.UTF8.GetBytes(input)));
		}
		finally
		{
			cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
		}
	}

	public static string CreateEncodedSignature(string input, SigningCredentials signingCredentials, bool cacheProvider)
	{
		if (input == null)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (signingCredentials == null)
		{
			throw LogHelper.LogArgumentNullException("signingCredentials");
		}
		CryptoProviderFactory cryptoProviderFactory = signingCredentials.CryptoProviderFactory ?? signingCredentials.Key.CryptoProviderFactory;
		SignatureProvider signatureProvider = cryptoProviderFactory.CreateForSigning(signingCredentials.Key, signingCredentials.Algorithm, cacheProvider);
		if (signatureProvider == null)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10637: CryptoProviderFactory.CreateForSigning returned null for key: '{0}', signatureAlgorithm: '{1}'.", (signingCredentials.Key == null) ? "Null" : signingCredentials.Key.ToString(), signingCredentials.Algorithm ?? "Null")));
		}
		try
		{
			LogHelper.LogVerbose(LogHelper.FormatInvariant("IDX14201: Creating raw signature using the signature credentials. Caching SignatureProvider: '{0}'.", cacheProvider));
			return Base64UrlEncoder.Encode(signatureProvider.Sign(Encoding.UTF8.GetBytes(input)));
		}
		finally
		{
			cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
		}
	}

	internal static string DecompressToken(byte[] tokenBytes, string algorithm)
	{
		if (tokenBytes == null)
		{
			throw LogHelper.LogArgumentNullException("tokenBytes");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (!CompressionProviderFactory.Default.IsSupportedAlgorithm(algorithm))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10682: Compression algorithm '{0}' is not supported.", algorithm)));
		}
		byte[] array = CompressionProviderFactory.Default.CreateCompressionProvider(algorithm).Decompress(tokenBytes);
		if (array == null)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecompressionFailedException(LogHelper.FormatInvariant("IDX10679: Failed to decompress using algorithm '{0}'.", algorithm)));
		}
		return Encoding.UTF8.GetString(array);
	}

	internal static string DecryptJwtToken(SecurityToken jwtToken, TokenValidationParameters validationParameters, JwtTokenDecryptionParameters decryptionParameters)
	{
		if (validationParameters == null)
		{
			throw LogHelper.LogArgumentNullException("validationParameters");
		}
		if (decryptionParameters == null)
		{
			throw LogHelper.LogArgumentNullException("decryptionParameters");
		}
		bool decryptionSucceeded = false;
		bool algorithmNotSupportedByCryptoProvider = false;
		byte[] array = null;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (SecurityKey key in decryptionParameters.Keys)
		{
			CryptoProviderFactory cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
			if (cryptoProviderFactory == null)
			{
				LogHelper.LogWarning("IDX10607: Decryption skipping key: '{0}', both validationParameters.CryptoProviderFactory and key.CryptoProviderFactory are null.", key);
				continue;
			}
			if (!cryptoProviderFactory.IsSupportedAlgorithm(decryptionParameters.Enc, key))
			{
				algorithmNotSupportedByCryptoProvider = true;
				LogHelper.LogWarning("IDX10611: Decryption failed. Encryption is not supported for: Algorithm: '{0}', SecurityKey: '{1}'.", decryptionParameters.Enc, key);
				continue;
			}
			try
			{
				Validators.ValidateAlgorithm(decryptionParameters.Enc, key, jwtToken, validationParameters);
				array = DecryptToken(cryptoProviderFactory, key, decryptionParameters);
				decryptionSucceeded = true;
			}
			catch (Exception ex)
			{
				stringBuilder.AppendLine(ex.ToString());
				goto IL_00dc;
			}
			break;
			IL_00dc:
			if (key != null)
			{
				stringBuilder2.AppendLine(key.ToString());
			}
		}
		ValidateDecryption(decryptionParameters, decryptionSucceeded, algorithmNotSupportedByCryptoProvider, stringBuilder, stringBuilder2);
		if (string.IsNullOrEmpty(decryptionParameters.Zip))
		{
			return Encoding.UTF8.GetString(array);
		}
		try
		{
			return decryptionParameters.DecompressionFunction(array, decryptionParameters.Zip);
		}
		catch (Exception inner)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecompressionFailedException(LogHelper.FormatInvariant("IDX10679: Failed to decompress using algorithm '{0}'.", decryptionParameters.Zip), inner));
		}
	}

	private static void ValidateDecryption(JwtTokenDecryptionParameters decryptionParameters, bool decryptionSucceeded, bool algorithmNotSupportedByCryptoProvider, StringBuilder exceptionStrings, StringBuilder keysAttempted)
	{
		if (!decryptionSucceeded && keysAttempted.Length > 0)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant("IDX10603: Decryption failed. Keys tried: '{0}'.\nExceptions caught:\n '{1}'.\ntoken: '{2}'", keysAttempted, exceptionStrings, decryptionParameters.EncodedToken)));
		}
		if (!decryptionSucceeded && algorithmNotSupportedByCryptoProvider)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant("IDX10619: Decryption failed. Algorithm: '{0}'. Either the Encryption Algorithm: '{1}' or none of the Security Keys are supported by the CryptoProviderFactory.", decryptionParameters.Alg, decryptionParameters.Enc)));
		}
		if (!decryptionSucceeded)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant("IDX10609: Decryption failed. No Keys tried: token: '{0}'.", decryptionParameters.EncodedToken)));
		}
	}

	private static byte[] DecryptToken(CryptoProviderFactory cryptoProviderFactory, SecurityKey key, JwtTokenDecryptionParameters decryptionParameters)
	{
		using AuthenticatedEncryptionProvider authenticatedEncryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(key, decryptionParameters.Enc);
		if (authenticatedEncryptionProvider == null)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10610: Decryption failed. Could not create decryption provider. Key: '{0}', Algorithm: '{1}'.", key, decryptionParameters.Enc)));
		}
		return authenticatedEncryptionProvider.Decrypt(Base64UrlEncoder.DecodeBytes(decryptionParameters.Ciphertext), Encoding.ASCII.GetBytes(decryptionParameters.EncodedHeader), Base64UrlEncoder.DecodeBytes(decryptionParameters.InitializationVector), Base64UrlEncoder.DecodeBytes(decryptionParameters.AuthenticationTag));
	}

	public static byte[] GenerateKeyBytes(int sizeInBits)
	{
		byte[] array = null;
		if (sizeInBits != 256 && sizeInBits != 384 && sizeInBits != 512)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException("IDX10401: Invalid requested key size. Valid key sizes are: 256, 384, and 512.", "sizeInBits"));
		}
		using Aes aes = Aes.Create();
		int num = sizeInBits >> 4;
		array = new byte[num << 1];
		aes.KeySize = sizeInBits >> 1;
		aes.GenerateKey();
		Array.Copy(aes.Key, array, num);
		aes.GenerateKey();
		Array.Copy(aes.Key, 0, array, num, num);
		return array;
	}

	internal static SecurityKey GetSecurityKey(EncryptingCredentials encryptingCredentials, CryptoProviderFactory cryptoProviderFactory, out byte[] wrappedKey)
	{
		SecurityKey securityKey = null;
		KeyWrapProvider keyWrapProvider = null;
		wrappedKey = null;
		if ("dir".Equals(encryptingCredentials.Alg, StringComparison.Ordinal))
		{
			if (!cryptoProviderFactory.IsSupportedAlgorithm(encryptingCredentials.Enc, encryptingCredentials.Key))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant("IDX10615: Encryption failed. No support for: Algorithm: '{0}', SecurityKey: '{1}'.", encryptingCredentials.Enc, encryptingCredentials.Key)));
			}
			securityKey = encryptingCredentials.Key;
		}
		else
		{
			if (!cryptoProviderFactory.IsSupportedAlgorithm(encryptingCredentials.Alg, encryptingCredentials.Key))
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant("IDX10615: Encryption failed. No support for: Algorithm: '{0}', SecurityKey: '{1}'.", encryptingCredentials.Alg, encryptingCredentials.Key)));
			}
			if ("A128CBC-HS256".Equals(encryptingCredentials.Enc, StringComparison.Ordinal))
			{
				securityKey = new SymmetricSecurityKey(GenerateKeyBytes(256));
			}
			else if ("A192CBC-HS384".Equals(encryptingCredentials.Enc, StringComparison.Ordinal))
			{
				securityKey = new SymmetricSecurityKey(GenerateKeyBytes(384));
			}
			else
			{
				if (!"A256CBC-HS512".Equals(encryptingCredentials.Enc, StringComparison.Ordinal))
				{
					throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant("IDX10617: Encryption failed. Keywrap is only supported for: '{0}', '{1}' and '{2}'. The content encryption specified is: '{3}'.", "A128CBC-HS256", "A192CBC-HS384", "A256CBC-HS512", encryptingCredentials.Enc)));
				}
				securityKey = new SymmetricSecurityKey(GenerateKeyBytes(512));
			}
			keyWrapProvider = cryptoProviderFactory.CreateKeyWrapProvider(encryptingCredentials.Key, encryptingCredentials.Alg);
			wrappedKey = keyWrapProvider.WrapKey(((SymmetricSecurityKey)securityKey).Key);
		}
		return securityKey;
	}

	public static IEnumerable<SecurityKey> GetAllDecryptionKeys(TokenValidationParameters validationParameters)
	{
		if (validationParameters == null)
		{
			throw new ArgumentNullException("validationParameters");
		}
		Collection<SecurityKey> collection = new Collection<SecurityKey>();
		if (validationParameters.TokenDecryptionKey != null)
		{
			collection.Add(validationParameters.TokenDecryptionKey);
		}
		if (validationParameters.TokenDecryptionKeys != null)
		{
			foreach (SecurityKey tokenDecryptionKey in validationParameters.TokenDecryptionKeys)
			{
				collection.Add(tokenDecryptionKey);
			}
		}
		return collection;
	}

	internal static DateTime GetDateTime(string key, JObject payload)
	{
		if (!payload.TryGetValue(key, out var value))
		{
			return DateTime.MinValue;
		}
		return EpochTime.DateTime(Convert.ToInt64(Math.Truncate(Convert.ToDouble(ParseTimeValue(value, key), CultureInfo.InvariantCulture))));
	}

	private static long ParseTimeValue(JToken jToken, string claimName)
	{
		if (jToken.Type == JTokenType.Integer || jToken.Type == JTokenType.Float)
		{
			return (long)jToken;
		}
		if (jToken.Type == JTokenType.String)
		{
			if (long.TryParse((string)jToken, out var result))
			{
				return result;
			}
			if (float.TryParse((string)jToken, out var result2))
			{
				return (long)result2;
			}
			if (double.TryParse((string)jToken, out var result3))
			{
				return (long)result3;
			}
		}
		throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant("IDX14300: Could not parse '{0}' : '{1}' as a '{2}'.", claimName, jToken.ToString(), typeof(long))));
	}

	internal static SecurityKey ResolveTokenSigningKey(string kid, string x5t, TokenValidationParameters validationParameters)
	{
		if (!string.IsNullOrEmpty(kid))
		{
			if (validationParameters.IssuerSigningKey != null && string.Equals(validationParameters.IssuerSigningKey.KeyId, kid, (validationParameters.IssuerSigningKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				return validationParameters.IssuerSigningKey;
			}
			if (validationParameters.IssuerSigningKeys != null)
			{
				foreach (SecurityKey issuerSigningKey in validationParameters.IssuerSigningKeys)
				{
					if (issuerSigningKey != null && string.Equals(issuerSigningKey.KeyId, kid, (issuerSigningKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
					{
						return issuerSigningKey;
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(x5t))
		{
			if (validationParameters.IssuerSigningKey != null)
			{
				if (string.Equals(validationParameters.IssuerSigningKey.KeyId, x5t, (validationParameters.IssuerSigningKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
				{
					return validationParameters.IssuerSigningKey;
				}
				if (validationParameters.IssuerSigningKey is X509SecurityKey x509SecurityKey && string.Equals(x509SecurityKey.X5t, x5t, StringComparison.OrdinalIgnoreCase))
				{
					return validationParameters.IssuerSigningKey;
				}
			}
			if (validationParameters.IssuerSigningKeys != null)
			{
				foreach (SecurityKey issuerSigningKey2 in validationParameters.IssuerSigningKeys)
				{
					if (issuerSigningKey2 != null && string.Equals(issuerSigningKey2.KeyId, x5t, StringComparison.Ordinal))
					{
						return issuerSigningKey2;
					}
				}
			}
		}
		return null;
	}
}
