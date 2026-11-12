using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class AuthenticatedEncryptionProvider : IDisposable
{
	private struct AuthenticatedKeys
	{
		public SymmetricSecurityKey AesKey;

		public SymmetricSecurityKey HmacKey;
	}

	private AuthenticatedKeys _authenticatedkeys;

	private CryptoProviderFactory _cryptoProviderFactory;

	private bool _disposed;

	private string _hmacAlgorithm;

	private SymmetricSignatureProvider _symmetricSignatureProvider;

	public string Algorithm { get; }

	public string Context { get; set; }

	public SecurityKey Key { get; }

	public AuthenticatedEncryptionProvider(SecurityKey key, string algorithm)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrWhiteSpace(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (!IsSupportedAlgorithm(key, algorithm))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10668: Unable to create '{0}', algorithm '{1}'; key: '{2}' is not supported.", GetType(), algorithm, key)));
		}
		ValidateKeySize(key, algorithm);
		_authenticatedkeys = GetAlgorithmParameters(key, algorithm);
		_hmacAlgorithm = GetHmacAlgorithm(algorithm);
		Key = key;
		Algorithm = algorithm;
		_cryptoProviderFactory = key.CryptoProviderFactory;
		if (key.CryptoProviderFactory.GetType() == typeof(CryptoProviderFactory))
		{
			_symmetricSignatureProvider = key.CryptoProviderFactory.CreateForSigning(_authenticatedkeys.HmacKey, _hmacAlgorithm, cacheProvider: false) as SymmetricSignatureProvider;
		}
		else
		{
			_symmetricSignatureProvider = key.CryptoProviderFactory.CreateForSigning(_authenticatedkeys.HmacKey, _hmacAlgorithm) as SymmetricSignatureProvider;
		}
		if (_symmetricSignatureProvider == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10649: Failed to create a SymmetricSignatureProvider for the algorithm '{0}'.", Algorithm)));
		}
	}

	public virtual AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData)
	{
		return Encrypt(plaintext, authenticatedData, null);
	}

	public virtual AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData, byte[] iv)
	{
		if (plaintext == null || plaintext.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("plaintext");
		}
		if (authenticatedData == null || authenticatedData.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("authenticatedData");
		}
		if (_disposed)
		{
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		using Aes aes = Aes.Create();
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		aes.Key = _authenticatedkeys.AesKey.Key;
		if (iv != null)
		{
			aes.IV = iv;
		}
		byte[] array;
		try
		{
			array = Transform(aes.CreateEncryptor(), plaintext, 0, plaintext.Length);
		}
		catch (Exception ex)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant("IDX10654: Decryption failed. Cryptographic operation exception: '{0}'.", ex)));
		}
		byte[] array2 = Utility.ConvertToBigEndian(authenticatedData.Length * 8);
		byte[] array3 = new byte[authenticatedData.Length + aes.IV.Length + array.Length + array2.Length];
		Array.Copy(authenticatedData, 0, array3, 0, authenticatedData.Length);
		Array.Copy(aes.IV, 0, array3, authenticatedData.Length, aes.IV.Length);
		Array.Copy(array, 0, array3, authenticatedData.Length + aes.IV.Length, array.Length);
		Array.Copy(array2, 0, array3, authenticatedData.Length + aes.IV.Length + array.Length, array2.Length);
		byte[] sourceArray = _symmetricSignatureProvider.Sign(array3);
		byte[] array4 = new byte[_authenticatedkeys.HmacKey.Key.Length];
		Array.Copy(sourceArray, array4, array4.Length);
		return new AuthenticatedEncryptionResult(Key, array, aes.IV, array4);
	}

	public virtual byte[] Decrypt(byte[] ciphertext, byte[] authenticatedData, byte[] iv, byte[] authenticationTag)
	{
		if (ciphertext == null || ciphertext.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("ciphertext");
		}
		if (authenticatedData == null || authenticatedData.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("authenticatedData");
		}
		if (iv == null || iv.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("iv");
		}
		if (authenticationTag == null || authenticationTag.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("authenticationTag");
		}
		if (_disposed)
		{
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		byte[] array = Utility.ConvertToBigEndian(authenticatedData.Length * 8);
		byte[] array2 = new byte[authenticatedData.Length + iv.Length + ciphertext.Length + array.Length];
		Array.Copy(authenticatedData, 0, array2, 0, authenticatedData.Length);
		Array.Copy(iv, 0, array2, authenticatedData.Length, iv.Length);
		Array.Copy(ciphertext, 0, array2, authenticatedData.Length + iv.Length, ciphertext.Length);
		Array.Copy(array, 0, array2, authenticatedData.Length + iv.Length + ciphertext.Length, array.Length);
		if (!_symmetricSignatureProvider.Verify(array2, authenticationTag, _authenticatedkeys.HmacKey.Key.Length))
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant("IDX10650: Failed to verify ciphertext with aad '{0}'; iv '{1}'; and authenticationTag '{2}'.", Base64UrlEncoder.Encode(authenticatedData), Base64UrlEncoder.Encode(iv), Base64UrlEncoder.Encode(authenticationTag))));
		}
		using Aes aes = Aes.Create();
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;
		aes.Key = _authenticatedkeys.AesKey.Key;
		aes.IV = iv;
		try
		{
			return Transform(aes.CreateDecryptor(), ciphertext, 0, ciphertext.Length);
		}
		catch (Exception ex)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenDecryptionFailedException(LogHelper.FormatInvariant("IDX10654: Decryption failed. Cryptographic operation exception: '{0}'.", ex)));
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_disposed = true;
			if (disposing && _symmetricSignatureProvider != null)
			{
				_cryptoProviderFactory.ReleaseSignatureProvider(_symmetricSignatureProvider);
			}
		}
	}

	protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
	{
		return SupportedAlgorithms.IsSupportedAuthenticatedEncryptionAlgorithm(algorithm, key);
	}

	private AuthenticatedKeys GetAlgorithmParameters(SecurityKey key, string algorithm)
	{
		int num = -1;
		if (algorithm.Equals("A256CBC-HS512", StringComparison.Ordinal))
		{
			num = 32;
		}
		else if (algorithm.Equals("A192CBC-HS384", StringComparison.Ordinal))
		{
			num = 24;
		}
		else
		{
			if (!algorithm.Equals("A128CBC-HS256", StringComparison.Ordinal))
			{
				throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10668: Unable to create '{0}', algorithm '{1}'; key: '{2}' is not supported.", GetType(), algorithm, key)));
			}
			num = 16;
		}
		byte[] keyBytes = GetKeyBytes(key);
		byte[] array = new byte[num];
		byte[] array2 = new byte[num];
		Array.Copy(keyBytes, num, array, 0, num);
		Array.Copy(keyBytes, array2, num);
		AuthenticatedKeys result = default(AuthenticatedKeys);
		result.AesKey = new SymmetricSecurityKey(array);
		result.HmacKey = new SymmetricSecurityKey(array2);
		return result;
	}

	private static string GetHmacAlgorithm(string algorithm)
	{
		if ("A128CBC-HS256".Equals(algorithm, StringComparison.Ordinal))
		{
			return "HS256";
		}
		if ("A192CBC-HS384".Equals(algorithm, StringComparison.Ordinal))
		{
			return "HS384";
		}
		if ("A256CBC-HS512".Equals(algorithm, StringComparison.Ordinal))
		{
			return "HS512";
		}
		throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm), "algorithm"));
	}

	protected virtual byte[] GetKeyBytes(SecurityKey key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (key is SymmetricSecurityKey symmetricSecurityKey)
		{
			return symmetricSecurityKey.Key;
		}
		if (key is JsonWebKey { K: not null, Kty: "oct" } jsonWebKey)
		{
			return Base64UrlEncoder.DecodeBytes(jsonWebKey.K);
		}
		throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10667: Unable to obtain required byte array for KeyHashAlgorithm from SecurityKey: '{0}'.", key)));
	}

	internal static byte[] Transform(ICryptoTransform transform, byte[] input, int inputOffset, int inputLength)
	{
		if (transform.CanTransformMultipleBlocks)
		{
			return transform.TransformFinalBlock(input, inputOffset, inputLength);
		}
		using MemoryStream memoryStream = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
		cryptoStream.Write(input, inputOffset, inputLength);
		cryptoStream.FlushFinalBlock();
		return memoryStream.ToArray();
	}

	protected virtual void ValidateKeySize(SecurityKey key, string algorithm)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if ("A128CBC-HS256".Equals(algorithm, StringComparison.Ordinal))
		{
			if (key.KeySize < 256)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10653: The encryption algorithm '{0}' requires a key size of at least '{1}' bits. Key '{2}', is of size: '{3}'.", "A128CBC-HS256", 256, key.KeyId, key.KeySize)));
			}
			return;
		}
		if ("A192CBC-HS384".Equals(algorithm, StringComparison.Ordinal))
		{
			if (key.KeySize >= 384)
			{
				return;
			}
			throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10653: The encryption algorithm '{0}' requires a key size of at least '{1}' bits. Key '{2}', is of size: '{3}'.", "A192CBC-HS384", 384, key.KeyId, key.KeySize)));
		}
		if ("A256CBC-HS512".Equals(algorithm, StringComparison.Ordinal))
		{
			if (key.KeySize >= 512)
			{
				return;
			}
			throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10653: The encryption algorithm '{0}' requires a key size of at least '{1}' bits. Key '{2}', is of size: '{3}'.", "A256CBC-HS512", 512, key.KeyId, key.KeySize)));
		}
		throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm)));
	}
}
