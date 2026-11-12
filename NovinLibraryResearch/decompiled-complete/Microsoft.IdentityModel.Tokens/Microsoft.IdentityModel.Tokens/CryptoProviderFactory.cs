using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class CryptoProviderFactory
{
	private static CryptoProviderFactory _default;

	private static ConcurrentDictionary<string, string> _typeToAlgorithmMap;

	private static object _cacheLock;

	private static int _defaultSignatureProviderObjectPoolCacheSize;

	private int _signatureProviderObjectPoolCacheSize = _defaultSignatureProviderObjectPoolCacheSize;

	public static CryptoProviderFactory Default
	{
		get
		{
			return _default;
		}
		set
		{
			_default = value ?? throw LogHelper.LogArgumentNullException("value");
		}
	}

	[DefaultValue(true)]
	public static bool DefaultCacheSignatureProviders { get; set; }

	public static int DefaultSignatureProviderObjectPoolCacheSize
	{
		get
		{
			return _defaultSignatureProviderObjectPoolCacheSize;
		}
		set
		{
			if (value <= 0)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX10698: The SignatureProviderObjectPoolCacheSize must be greater than 0. Value: '{0}'.", value)));
			}
			_defaultSignatureProviderObjectPoolCacheSize = value;
		}
	}

	public CryptoProviderCache CryptoProviderCache { get; } = new InMemoryCryptoProviderCache();


	public ICryptoProvider CustomCryptoProvider { get; set; }

	[DefaultValue(true)]
	public bool CacheSignatureProviders { get; set; } = DefaultCacheSignatureProviders;


	public int SignatureProviderObjectPoolCacheSize
	{
		get
		{
			return _signatureProviderObjectPoolCacheSize;
		}
		set
		{
			if (value <= 0)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX10698: The SignatureProviderObjectPoolCacheSize must be greater than 0. Value: '{0}'.", value)));
			}
			_signatureProviderObjectPoolCacheSize = value;
		}
	}

	static CryptoProviderFactory()
	{
		_typeToAlgorithmMap = new ConcurrentDictionary<string, string>();
		_cacheLock = new object();
		_defaultSignatureProviderObjectPoolCacheSize = Environment.ProcessorCount * 4;
		DefaultCacheSignatureProviders = true;
		Default = new CryptoProviderFactory();
	}

	public CryptoProviderFactory()
	{
	}

	public CryptoProviderFactory(CryptoProviderFactory other)
	{
		if (other == null)
		{
			throw LogHelper.LogArgumentNullException("other");
		}
		CustomCryptoProvider = other.CustomCryptoProvider;
		CacheSignatureProviders = other.CacheSignatureProviders;
		SignatureProviderObjectPoolCacheSize = other.SignatureProviderObjectPoolCacheSize;
	}

	public virtual AuthenticatedEncryptionProvider CreateAuthenticatedEncryptionProvider(SecurityKey key, string algorithm)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
		{
			if (!(CustomCryptoProvider.Create(algorithm, key) is AuthenticatedEncryptionProvider result))
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10646: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}', Key: '{1}'), but Create.(algorithm, args) as '{2}' == NULL.", algorithm, key, typeof(AuthenticatedEncryptionProvider))));
			}
			return result;
		}
		if (SupportedAlgorithms.IsSupportedAuthenticatedEncryptionAlgorithm(algorithm, key))
		{
			return new AuthenticatedEncryptionProvider(key, algorithm);
		}
		throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm), "algorithm"));
	}

	public virtual KeyWrapProvider CreateKeyWrapProvider(SecurityKey key, string algorithm)
	{
		return CreateKeyWrapProvider(key, algorithm, willUnwrap: false);
	}

	public virtual KeyWrapProvider CreateKeyWrapProviderForUnwrap(SecurityKey key, string algorithm)
	{
		return CreateKeyWrapProvider(key, algorithm, willUnwrap: true);
	}

	private KeyWrapProvider CreateKeyWrapProvider(SecurityKey key, string algorithm, bool willUnwrap)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key, willUnwrap))
		{
			if (!(CustomCryptoProvider.Create(algorithm, key, willUnwrap) is KeyWrapProvider result))
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10646: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}', Key: '{1}'), but Create.(algorithm, args) as '{2}' == NULL.", algorithm, key, typeof(SignatureProvider))));
			}
			return result;
		}
		if (SupportedAlgorithms.IsSupportedRsaKeyWrap(algorithm, key))
		{
			return new RsaKeyWrapProvider(key, algorithm, willUnwrap);
		}
		if (SupportedAlgorithms.IsSupportedSymmetricKeyWrap(algorithm, key))
		{
			return new SymmetricKeyWrapProvider(key, algorithm);
		}
		throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10661: Unable to create the KeyWrapProvider.\nKeyWrapAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported.", algorithm, key)));
	}

	public virtual SignatureProvider CreateForSigning(SecurityKey key, string algorithm)
	{
		return CreateForSigning(key, algorithm, CacheSignatureProviders);
	}

	public virtual SignatureProvider CreateForSigning(SecurityKey key, string algorithm, bool cacheProvider)
	{
		return CreateSignatureProvider(key, algorithm, willCreateSignatures: true, cacheProvider);
	}

	public virtual SignatureProvider CreateForVerifying(SecurityKey key, string algorithm)
	{
		return CreateForVerifying(key, algorithm, CacheSignatureProviders);
	}

	public virtual SignatureProvider CreateForVerifying(SecurityKey key, string algorithm, bool cacheProvider)
	{
		return CreateSignatureProvider(key, algorithm, willCreateSignatures: false, cacheProvider);
	}

	public virtual HashAlgorithm CreateHashAlgorithm(HashAlgorithmName algorithm)
	{
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm.Name))
		{
			if (!(CustomCryptoProvider.Create(algorithm.Name) is HashAlgorithm hashAlgorithm))
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10647: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}'), but Create.(algorithm, args) as '{1}' == NULL.", algorithm, typeof(HashAlgorithm))));
			}
			_typeToAlgorithmMap[hashAlgorithm.GetType().ToString()] = algorithm.Name;
			return hashAlgorithm;
		}
		if (algorithm == HashAlgorithmName.SHA256)
		{
			return SHA256.Create();
		}
		if (algorithm == HashAlgorithmName.SHA384)
		{
			return SHA384.Create();
		}
		if (algorithm == HashAlgorithmName.SHA512)
		{
			return SHA512.Create();
		}
		throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10640: Algorithm is not supported: '{0}'.", algorithm)));
	}

	public virtual HashAlgorithm CreateHashAlgorithm(string algorithm)
	{
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
		{
			if (!(CustomCryptoProvider.Create(algorithm) is HashAlgorithm hashAlgorithm))
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10647: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}'), but Create.(algorithm, args) as '{1}' == NULL.", algorithm, typeof(HashAlgorithm))));
			}
			_typeToAlgorithmMap[hashAlgorithm.GetType().ToString()] = algorithm;
			return hashAlgorithm;
		}
		switch (algorithm)
		{
		case "SHA256":
		case "http://www.w3.org/2001/04/xmlenc#sha256":
			return SHA256.Create();
		case "SHA384":
		case "http://www.w3.org/2001/04/xmldsig-more#sha384":
			return SHA384.Create();
		case "SHA512":
		case "http://www.w3.org/2001/04/xmlenc#sha512":
			return SHA512.Create();
		default:
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10640: Algorithm is not supported: '{0}'.", algorithm)));
		}
	}

	public virtual KeyedHashAlgorithm CreateKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
	{
		if (keyBytes == null)
		{
			throw LogHelper.LogArgumentNullException("keyBytes");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, keyBytes))
		{
			if (!(CustomCryptoProvider.Create(algorithm, keyBytes) is KeyedHashAlgorithm result))
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10647: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}'), but Create.(algorithm, args) as '{1}' == NULL.", algorithm, typeof(KeyedHashAlgorithm))));
			}
			return result;
		}
		switch (algorithm)
		{
		case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256":
		case "HS256":
			return new HMACSHA256(keyBytes);
		case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384":
		case "HS384":
			return new HMACSHA384(keyBytes);
		case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512":
		case "HS512":
			return new HMACSHA512(keyBytes);
		default:
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10666: Unable to create KeyedHashAlgorithm for algorithm '{0}'.", algorithm)));
		}
	}

	private SignatureProvider CreateSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures, bool cacheProvider)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key, willCreateSignatures))
		{
			if (!(CustomCryptoProvider.Create(algorithm, key, willCreateSignatures) is SignatureProvider result))
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10646: A CustomCryptoProvider was set and returned 'true' for IsSupportedAlgorithm(Algorithm: '{0}', Key: '{1}'), but Create.(algorithm, args) as '{2}' == NULL.", algorithm, key, typeof(SignatureProvider))));
			}
			return result;
		}
		string text = null;
		bool flag = true;
		if (key is AsymmetricSecurityKey)
		{
			text = typeof(AsymmetricSignatureProvider).ToString();
		}
		else if (key is JsonWebKey jsonWebKey)
		{
			try
			{
				if (JsonWebKeyConverter.TryConvertToSecurityKey(jsonWebKey, out var key2))
				{
					if (key2 is AsymmetricSecurityKey)
					{
						text = typeof(AsymmetricSignatureProvider).ToString();
					}
					else if (key2 is SymmetricSecurityKey)
					{
						text = typeof(SymmetricSignatureProvider).ToString();
						flag = false;
					}
				}
				else if (jsonWebKey.Kty == "RSA" || jsonWebKey.Kty == "EC")
				{
					text = typeof(AsymmetricSignatureProvider).ToString();
				}
				else if (jsonWebKey.Kty == "oct")
				{
					text = typeof(SymmetricSignatureProvider).ToString();
					flag = false;
				}
			}
			catch (Exception ex)
			{
				throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant("IDX10694: JsonWebKeyConverter threw attempting to convert JsonWebKey: '{0}'. Exception: '{1}'.", key, ex), ex));
			}
		}
		else if (key is SymmetricSecurityKey)
		{
			text = typeof(SymmetricSignatureProvider).ToString();
			flag = false;
		}
		if (text == null)
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10621: '{0}' supports: '{1}' of types: '{2}' or '{3}'. SecurityKey received was of type '{4}'.", typeof(SymmetricSignatureProvider), typeof(SecurityKey), typeof(AsymmetricSecurityKey), typeof(SymmetricSecurityKey), key.GetType())));
		}
		if (!IsSupportedAlgorithm(algorithm, key))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10634: Unable to create the SignatureProvider.\nAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported. The list of supported algorithms is available here: https://aka.ms/IdentityModel/supported-algorithms", algorithm, key)));
		}
		SignatureProvider signatureProvider;
		if (!(CacheSignatureProviders && cacheProvider))
		{
			signatureProvider = ((!flag) ? ((SignatureProvider)new SymmetricSignatureProvider(key, algorithm, willCreateSignatures)) : ((SignatureProvider)new AsymmetricSignatureProvider(key, algorithm, willCreateSignatures)));
		}
		else
		{
			if (CryptoProviderCache.TryGetSignatureProvider(key, algorithm, text, willCreateSignatures, out signatureProvider))
			{
				return signatureProvider;
			}
			lock (_cacheLock)
			{
				if (CryptoProviderCache.TryGetSignatureProvider(key, algorithm, text, willCreateSignatures, out signatureProvider))
				{
					return signatureProvider;
				}
				signatureProvider = ((!flag) ? ((SignatureProvider)new SymmetricSignatureProvider(key, algorithm, willCreateSignatures)) : ((SignatureProvider)new AsymmetricSignatureProvider(key, algorithm, willCreateSignatures, this)));
				CryptoProviderCache.TryAdd(signatureProvider);
			}
		}
		return signatureProvider;
	}

	public virtual bool IsSupportedAlgorithm(string algorithm)
	{
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
		{
			return true;
		}
		return SupportedAlgorithms.IsSupportedHashAlgorithm(algorithm);
	}

	public virtual bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
	{
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
		{
			return true;
		}
		return SupportedAlgorithms.IsSupportedAlgorithm(algorithm, (key is JsonWebKey { ConvertedSecurityKey: not null } jsonWebKey) ? jsonWebKey.ConvertedSecurityKey : key);
	}

	public virtual void ReleaseHashAlgorithm(HashAlgorithm hashAlgorithm)
	{
		if (hashAlgorithm == null)
		{
			throw LogHelper.LogArgumentNullException("hashAlgorithm");
		}
		if (CustomCryptoProvider != null && _typeToAlgorithmMap.TryGetValue(hashAlgorithm.GetType().ToString(), out var value) && CustomCryptoProvider.IsSupportedAlgorithm(value))
		{
			CustomCryptoProvider.Release(hashAlgorithm);
		}
		else
		{
			hashAlgorithm.Dispose();
		}
	}

	public virtual void ReleaseKeyWrapProvider(KeyWrapProvider provider)
	{
		if (provider == null)
		{
			throw LogHelper.LogArgumentNullException("provider");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(provider.Algorithm))
		{
			CustomCryptoProvider.Release(provider);
		}
		else
		{
			provider.Dispose();
		}
	}

	public virtual void ReleaseRsaKeyWrapProvider(RsaKeyWrapProvider provider)
	{
		if (provider == null)
		{
			throw LogHelper.LogArgumentNullException("provider");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(provider.Algorithm))
		{
			CustomCryptoProvider.Release(provider);
		}
		else
		{
			provider.Dispose();
		}
	}

	public virtual void ReleaseSignatureProvider(SignatureProvider signatureProvider)
	{
		if (signatureProvider == null)
		{
			throw LogHelper.LogArgumentNullException("signatureProvider");
		}
		if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(signatureProvider.Algorithm))
		{
			CustomCryptoProvider.Release(signatureProvider);
		}
		else if (signatureProvider.CryptoProviderCache == null)
		{
			signatureProvider.Dispose();
		}
	}
}
