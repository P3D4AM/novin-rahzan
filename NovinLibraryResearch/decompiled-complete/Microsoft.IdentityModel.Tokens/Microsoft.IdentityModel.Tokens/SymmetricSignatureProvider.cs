using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class SymmetricSignatureProvider : SignatureProvider
{
	private bool _disposed;

	private DisposableObjectPool<KeyedHashAlgorithm> _keyedHashObjectPool;

	public static readonly int DefaultMinimumSymmetricKeySizeInBits = 128;

	private int _minimumSymmetricKeySizeInBits = DefaultMinimumSymmetricKeySizeInBits;

	public int MinimumSymmetricKeySizeInBits
	{
		get
		{
			return _minimumSymmetricKeySizeInBits;
		}
		set
		{
			if (value < DefaultMinimumSymmetricKeySizeInBits)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX10628: Cannot set the MinimumSymmetricKeySizeInBits to less than '{0}'.", DefaultMinimumSymmetricKeySizeInBits)));
			}
			_minimumSymmetricKeySizeInBits = value;
		}
	}

	internal override int ObjectPoolSize => _keyedHashObjectPool.Size;

	public SymmetricSignatureProvider(SecurityKey key, string algorithm)
		: this(key, algorithm, willCreateSignatures: true)
	{
	}

	public SymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
		: base(key, algorithm)
	{
		if (!key.CryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10634: Unable to create the SignatureProvider.\nAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms", algorithm ?? "null", key)));
		}
		if (key.KeySize < MinimumSymmetricKeySizeInBits)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("key", LogHelper.FormatInvariant("IDX10653: The encryption algorithm '{0}' requires a key size of at least '{1}' bits. Key '{2}', is of size: '{3}'.", algorithm ?? "null", MinimumSymmetricKeySizeInBits, key, key.KeySize)));
		}
		base.WillCreateSignatures = willCreateSignatures;
		_keyedHashObjectPool = new DisposableObjectPool<KeyedHashAlgorithm>(CreateKeyedHashAlgorithm, key.CryptoProviderFactory.SignatureProviderObjectPoolCacheSize);
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

	protected virtual KeyedHashAlgorithm GetKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
	{
		return _keyedHashObjectPool.Allocate();
	}

	private KeyedHashAlgorithm CreateKeyedHashAlgorithm()
	{
		return base.Key.CryptoProviderFactory.CreateKeyedHashAlgorithm(GetKeyBytes(base.Key), base.Algorithm);
	}

	protected virtual void ReleaseKeyedHashAlgorithm(KeyedHashAlgorithm keyedHashAlgorithm)
	{
		_keyedHashObjectPool.Free(keyedHashAlgorithm);
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
		LogHelper.LogInformation("IDX10642: Creating signature using the input: '{0}'.", input);
		KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(base.Key), base.Algorithm);
		try
		{
			return keyedHashAlgorithm.ComputeHash(input);
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
				ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
			}
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
		LogHelper.LogInformation("IDX10643: Comparing the signature created over the input with the token signature: '{0}'.", input);
		KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(base.Key), base.Algorithm);
		try
		{
			return Utility.AreEqual(signature, keyedHashAlgorithm.ComputeHash(input));
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
				ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
			}
		}
	}

	public bool Verify(byte[] input, byte[] signature, int length)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (signature == null || signature.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("signature");
		}
		if (length < 1)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10655: 'length' must be greater than 1: '{0}'", length)));
		}
		if (_disposed)
		{
			base.CryptoProviderCache?.TryRemove(this);
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		LogHelper.LogInformation("IDX10643: Comparing the signature created over the input with the token signature: '{0}'.", input);
		KeyedHashAlgorithm keyedHashAlgorithm = GetKeyedHashAlgorithm(GetKeyBytes(base.Key), base.Algorithm);
		try
		{
			return Utility.AreEqual(signature, keyedHashAlgorithm.ComputeHash(input), length);
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
				ReleaseKeyedHashAlgorithm(keyedHashAlgorithm);
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
			DisposableObjectPool<KeyedHashAlgorithm>.Element[] items = _keyedHashObjectPool.Items;
			for (int i = 0; i < items.Length; i++)
			{
				items[i].Value?.Dispose();
			}
			base.CryptoProviderCache?.TryRemove(this);
		}
	}
}
