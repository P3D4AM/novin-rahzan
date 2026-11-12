using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class InMemoryCryptoProviderCache : CryptoProviderCache
{
	private ConcurrentDictionary<string, SignatureProvider> _signingSignatureProviders = new ConcurrentDictionary<string, SignatureProvider>();

	private ConcurrentDictionary<string, SignatureProvider> _verifyingSignatureProviders = new ConcurrentDictionary<string, SignatureProvider>();

	protected override string GetCacheKey(SignatureProvider signatureProvider)
	{
		if (signatureProvider == null)
		{
			throw LogHelper.LogArgumentNullException("signatureProvider");
		}
		return GetCacheKeyPrivate(signatureProvider.Key, signatureProvider.Algorithm, signatureProvider.GetType().ToString());
	}

	protected override string GetCacheKey(SecurityKey securityKey, string algorithm, string typeofProvider)
	{
		if (securityKey == null)
		{
			throw LogHelper.LogArgumentNullException("securityKey");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (string.IsNullOrEmpty(typeofProvider))
		{
			throw LogHelper.LogArgumentNullException("typeofProvider");
		}
		return GetCacheKeyPrivate(securityKey, algorithm, typeofProvider);
	}

	private static string GetCacheKeyPrivate(SecurityKey securityKey, string algorithm, string typeofProvider)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}-{3}", securityKey.GetType(), securityKey.InternalId, algorithm, typeofProvider);
	}

	private static bool ShouldCacheSignatureProvider(SignatureProvider signatureProvider)
	{
		return signatureProvider.Key.InternalId.Length != 0;
	}

	public override bool TryAdd(SignatureProvider signatureProvider)
	{
		if (signatureProvider == null)
		{
			throw LogHelper.LogArgumentNullException("signatureProvider");
		}
		if (!ShouldCacheSignatureProvider(signatureProvider))
		{
			return false;
		}
		string cacheKey = GetCacheKey(signatureProvider);
		if (signatureProvider.WillCreateSignatures)
		{
			if (_signingSignatureProviders.TryAdd(cacheKey, signatureProvider))
			{
				signatureProvider.CryptoProviderCache = this;
				return true;
			}
		}
		else if (_verifyingSignatureProviders.TryAdd(cacheKey, signatureProvider))
		{
			signatureProvider.CryptoProviderCache = this;
			return true;
		}
		return false;
	}

	public override bool TryGetSignatureProvider(SecurityKey securityKey, string algorithm, string typeofProvider, bool willCreateSignatures, out SignatureProvider signatureProvider)
	{
		if (securityKey == null)
		{
			throw LogHelper.LogArgumentNullException("securityKey");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (string.IsNullOrEmpty(typeofProvider))
		{
			throw LogHelper.LogArgumentNullException("typeofProvider");
		}
		string cacheKeyPrivate = GetCacheKeyPrivate(securityKey, algorithm, typeofProvider);
		if (willCreateSignatures)
		{
			return _signingSignatureProviders.TryGetValue(cacheKeyPrivate, out signatureProvider);
		}
		return _verifyingSignatureProviders.TryGetValue(cacheKeyPrivate, out signatureProvider);
	}

	public override bool TryRemove(SignatureProvider signatureProvider)
	{
		if (signatureProvider == null)
		{
			throw LogHelper.LogArgumentNullException("signatureProvider");
		}
		if (signatureProvider.CryptoProviderCache != this)
		{
			return false;
		}
		string cacheKey = GetCacheKey(signatureProvider);
		SignatureProvider value2;
		if (signatureProvider.WillCreateSignatures)
		{
			if (_signingSignatureProviders.TryRemove(cacheKey, out var value))
			{
				value.CryptoProviderCache = null;
				return true;
			}
		}
		else if (_verifyingSignatureProviders.TryRemove(cacheKey, out value2))
		{
			value2.CryptoProviderCache = null;
			return true;
		}
		return false;
	}
}
