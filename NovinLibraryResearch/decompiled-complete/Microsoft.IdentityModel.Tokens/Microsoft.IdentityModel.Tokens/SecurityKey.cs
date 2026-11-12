using System;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public abstract class SecurityKey
{
	private CryptoProviderFactory _cryptoProviderFactory;

	private Lazy<string> _internalId;

	[JsonIgnore]
	internal virtual string InternalId => _internalId.Value;

	public abstract int KeySize { get; }

	[JsonIgnore]
	public virtual string KeyId { get; set; }

	[JsonIgnore]
	public CryptoProviderFactory CryptoProviderFactory
	{
		get
		{
			return _cryptoProviderFactory;
		}
		set
		{
			_cryptoProviderFactory = value ?? throw LogHelper.LogArgumentNullException("value");
		}
	}

	internal SecurityKey(SecurityKey key)
	{
		_cryptoProviderFactory = key._cryptoProviderFactory;
		KeyId = key.KeyId;
		SetInternalId();
	}

	public SecurityKey()
	{
		_cryptoProviderFactory = CryptoProviderFactory.Default;
		SetInternalId();
	}

	public override string ToString()
	{
		return $"{GetType()}, KeyId: '{KeyId}', InternalId: '{InternalId}'.";
	}

	public virtual bool CanComputeJwkThumbprint()
	{
		return false;
	}

	public virtual byte[] ComputeJwkThumbprint()
	{
		throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10710: Computing a JWK thumbprint is supported only on SymmetricSecurityKey, JsonWebKey, RsaSecurityKey, X509SecurityKey, and ECDsaSecurityKey.")));
	}

	public virtual bool IsSupportedAlgorithm(string algorithm)
	{
		return CryptoProviderFactory.IsSupportedAlgorithm(algorithm, this);
	}

	private void SetInternalId()
	{
		_internalId = new Lazy<string>(() => CanComputeJwkThumbprint() ? Base64UrlEncoder.Encode(ComputeJwkThumbprint()) : string.Empty);
	}
}
