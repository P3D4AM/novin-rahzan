using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class AsymmetricSignatureProvider : SignatureProvider
{
	private DisposableObjectPool<AsymmetricAdapter> _asymmetricAdapterObjectPool;

	private CryptoProviderFactory _cryptoProviderFactory;

	private bool _disposed;

	private IReadOnlyDictionary<string, int> _minimumAsymmetricKeySizeInBitsForSigningMap;

	private IReadOnlyDictionary<string, int> _minimumAsymmetricKeySizeInBitsForVerifyingMap;

	public static readonly Dictionary<string, int> DefaultMinimumAsymmetricKeySizeInBitsForSigningMap = new Dictionary<string, int>
	{
		{ "ES256", 256 },
		{ "ES384", 256 },
		{ "ES512", 256 },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256", 256 },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384", 256 },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512", 256 },
		{ "RS256", 2048 },
		{ "RS384", 2048 },
		{ "RS512", 2048 },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", 2048 },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", 2048 },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", 2048 },
		{ "PS256", 528 },
		{ "PS384", 784 },
		{ "PS512", 1040 },
		{ "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1", 528 },
		{ "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1", 784 },
		{ "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1", 1040 }
	};

	public static readonly Dictionary<string, int> DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap = new Dictionary<string, int>
	{
		{ "ES256", 256 },
		{ "ES384", 256 },
		{ "ES512", 256 },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256", 256 },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384", 256 },
		{ "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512", 256 },
		{ "RS256", 1024 },
		{ "RS384", 1024 },
		{ "RS512", 1024 },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", 1024 },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", 1024 },
		{ "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", 1024 },
		{ "PS256", 528 },
		{ "PS384", 784 },
		{ "PS512", 1040 },
		{ "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1", 528 },
		{ "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1", 784 },
		{ "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1", 1040 }
	};

	public IReadOnlyDictionary<string, int> MinimumAsymmetricKeySizeInBitsForSigningMap => _minimumAsymmetricKeySizeInBitsForSigningMap;

	public IReadOnlyDictionary<string, int> MinimumAsymmetricKeySizeInBitsForVerifyingMap => _minimumAsymmetricKeySizeInBitsForVerifyingMap;

	internal override int ObjectPoolSize => _asymmetricAdapterObjectPool.Size;

	internal AsymmetricSignatureProvider(SecurityKey key, string algorithm, CryptoProviderFactory cryptoProviderFactory)
		: this(key, algorithm)
	{
		_cryptoProviderFactory = cryptoProviderFactory;
	}

	internal AsymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures, CryptoProviderFactory cryptoProviderFactory)
		: this(key, algorithm, willCreateSignatures)
	{
		_cryptoProviderFactory = cryptoProviderFactory;
	}

	public AsymmetricSignatureProvider(SecurityKey key, string algorithm)
		: this(key, algorithm, willCreateSignatures: false)
	{
	}

	public AsymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
		: base(key, algorithm)
	{
		_cryptoProviderFactory = key.CryptoProviderFactory;
		_minimumAsymmetricKeySizeInBitsForSigningMap = new Dictionary<string, int>(DefaultMinimumAsymmetricKeySizeInBitsForSigningMap);
		_minimumAsymmetricKeySizeInBitsForVerifyingMap = new Dictionary<string, int>(DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap);
		if (key is JsonWebKey webKey)
		{
			JsonWebKeyConverter.TryConvertToSecurityKey(webKey, out var _);
		}
		if (willCreateSignatures && FoundPrivateKey(key) == PrivateKeyStatus.DoesNotExist)
		{
			throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10638: Cannot create the SignatureProvider, 'key.HasPrivateKey' is false, cannot create signatures. Key: {0}.", key)));
		}
		if (!_cryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10634: Unable to create the SignatureProvider.\nAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms", algorithm ?? "null", key)));
		}
		base.WillCreateSignatures = willCreateSignatures;
		ValidateAsymmetricSecurityKeySize(key, algorithm, base.WillCreateSignatures);
		_asymmetricAdapterObjectPool = new DisposableObjectPool<AsymmetricAdapter>(CreateAsymmetricAdapter, _cryptoProviderFactory.SignatureProviderObjectPoolCacheSize);
	}

	private static PrivateKeyStatus FoundPrivateKey(SecurityKey key)
	{
		if (key is AsymmetricSecurityKey asymmetricSecurityKey)
		{
			return asymmetricSecurityKey.PrivateKeyStatus;
		}
		if (key is JsonWebKey jsonWebKey)
		{
			if (!jsonWebKey.HasPrivateKey)
			{
				return PrivateKeyStatus.DoesNotExist;
			}
			return PrivateKeyStatus.Exists;
		}
		return PrivateKeyStatus.Unknown;
	}

	protected virtual HashAlgorithmName GetHashAlgorithmName(string algorithm)
	{
		if (string.IsNullOrWhiteSpace(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		return SupportedAlgorithms.GetHashAlgorithmName(algorithm);
	}

	private AsymmetricAdapter CreateAsymmetricAdapter()
	{
		HashAlgorithmName hashAlgorithmName = GetHashAlgorithmName(base.Algorithm);
		return new AsymmetricAdapter(base.Key, base.Algorithm, _cryptoProviderFactory.CreateHashAlgorithm(hashAlgorithmName), hashAlgorithmName, base.WillCreateSignatures);
	}

	public override byte[] Sign(byte[] input)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (_disposed)
		{
			base.CryptoProviderCache?.TryRemove(this);
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		AsymmetricAdapter asymmetricAdapter = null;
		try
		{
			asymmetricAdapter = _asymmetricAdapterObjectPool.Allocate();
			return asymmetricAdapter.Sign(input);
		}
		catch
		{
			base.CryptoProviderCache?.TryRemove(this);
			Dispose(disposing: true);
			throw;
		}
		finally
		{
			if (!_disposed)
			{
				_asymmetricAdapterObjectPool.Free(asymmetricAdapter);
			}
		}
	}

	public virtual void ValidateAsymmetricSecurityKeySize(SecurityKey key, string algorithm, bool willCreateSignatures)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		int keySize = key.KeySize;
		if (key is AsymmetricSecurityKey asymmetricSecurityKey)
		{
			keySize = asymmetricSecurityKey.KeySize;
		}
		else
		{
			if (!(key is JsonWebKey webKey))
			{
				throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10704: Cannot verify the key size. The SecurityKey is not or cannot be converted to an AsymmetricSecuritKey. SecurityKey: '{0}'.", key)));
			}
			JsonWebKeyConverter.TryConvertToSecurityKey(webKey, out var key2);
			if (key2 is AsymmetricSecurityKey asymmetricSecurityKey2)
			{
				keySize = asymmetricSecurityKey2.KeySize;
			}
			else if (key2 is SymmetricSecurityKey)
			{
				throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10704: Cannot verify the key size. The SecurityKey is not or cannot be converted to an AsymmetricSecuritKey. SecurityKey: '{0}'.", key)));
			}
		}
		if (willCreateSignatures)
		{
			if (MinimumAsymmetricKeySizeInBitsForSigningMap.ContainsKey(algorithm) && keySize < MinimumAsymmetricKeySizeInBitsForSigningMap[algorithm])
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10630: The '{0}' for signing cannot be smaller than '{1}' bits. KeySize: '{2}'.", key, MinimumAsymmetricKeySizeInBitsForSigningMap[algorithm], keySize)));
			}
		}
		else if (MinimumAsymmetricKeySizeInBitsForVerifyingMap.ContainsKey(algorithm) && keySize < MinimumAsymmetricKeySizeInBitsForVerifyingMap[algorithm])
		{
			throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10631: The '{0}' for verifying cannot be smaller than '{1}' bits. KeySize: '{2}'.", key, MinimumAsymmetricKeySizeInBitsForVerifyingMap[algorithm], keySize)));
		}
	}

	public override bool Verify(byte[] input, byte[] signature)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (signature == null || signature.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("signature");
		}
		if (_disposed)
		{
			base.CryptoProviderCache?.TryRemove(this);
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		AsymmetricAdapter asymmetricAdapter = null;
		try
		{
			asymmetricAdapter = _asymmetricAdapterObjectPool.Allocate();
			return asymmetricAdapter.Verify(input, signature);
		}
		catch
		{
			base.CryptoProviderCache?.TryRemove(this);
			Dispose(disposing: true);
			throw;
		}
		finally
		{
			if (!_disposed)
			{
				_asymmetricAdapterObjectPool.Free(asymmetricAdapter);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		if (disposing)
		{
			DisposableObjectPool<AsymmetricAdapter>.Element[] items = _asymmetricAdapterObjectPool.Items;
			for (int i = 0; i < items.Length; i++)
			{
				items[i].Value?.Dispose();
			}
			base.CryptoProviderCache?.TryRemove(this);
		}
	}
}
