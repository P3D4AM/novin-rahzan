using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class X509SecurityKey : AsymmetricSecurityKey
{
	private AsymmetricAlgorithm _privateKey;

	private bool _privateKeyAvailabilityDetermined;

	private AsymmetricAlgorithm _publicKey;

	private object _thisLock = new object();

	public override int KeySize => PublicKey.KeySize;

	public string X5t { get; }

	public AsymmetricAlgorithm PrivateKey
	{
		get
		{
			if (!_privateKeyAvailabilityDetermined)
			{
				lock (ThisLock)
				{
					if (!_privateKeyAvailabilityDetermined)
					{
						_privateKey = Certificate.GetRSAPrivateKey();
						_privateKeyAvailabilityDetermined = true;
					}
				}
			}
			return _privateKey;
		}
	}

	public AsymmetricAlgorithm PublicKey
	{
		get
		{
			if (_publicKey == null)
			{
				lock (ThisLock)
				{
					if (_publicKey == null)
					{
						_publicKey = Certificate.GetRSAPublicKey();
					}
				}
			}
			return _publicKey;
		}
	}

	private object ThisLock => _thisLock;

	[Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus.")]
	public override bool HasPrivateKey => PrivateKey != null;

	public override PrivateKeyStatus PrivateKeyStatus
	{
		get
		{
			if (PrivateKey != null)
			{
				return PrivateKeyStatus.Exists;
			}
			return PrivateKeyStatus.DoesNotExist;
		}
	}

	public X509Certificate2 Certificate { get; private set; }

	internal override string InternalId => X5t;

	internal X509SecurityKey(JsonWebKey webKey)
		: base(webKey)
	{
		Certificate = new X509Certificate2(Convert.FromBase64String(webKey.X5c[0]));
		X5t = Base64UrlEncoder.Encode(Certificate.GetCertHash());
		webKey.ConvertedSecurityKey = this;
	}

	public X509SecurityKey(X509Certificate2 certificate)
	{
		Certificate = certificate ?? throw LogHelper.LogArgumentNullException("certificate");
		KeyId = certificate.Thumbprint;
		X5t = Base64UrlEncoder.Encode(certificate.GetCertHash());
	}

	public X509SecurityKey(X509Certificate2 certificate, string keyId)
	{
		Certificate = certificate ?? throw LogHelper.LogArgumentNullException("certificate");
		if (string.IsNullOrEmpty(keyId))
		{
			throw LogHelper.LogArgumentNullException("keyId");
		}
		KeyId = keyId;
		X5t = Base64UrlEncoder.Encode(certificate.GetCertHash());
	}

	public override bool CanComputeJwkThumbprint()
	{
		if (!(PublicKey is RSA))
		{
			return false;
		}
		return true;
	}

	public override byte[] ComputeJwkThumbprint()
	{
		return new RsaSecurityKey(PublicKey as RSA).ComputeJwkThumbprint();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is X509SecurityKey x509SecurityKey))
		{
			return false;
		}
		return x509SecurityKey.Certificate.Thumbprint.ToString() == Certificate.Thumbprint.ToString();
	}

	public override int GetHashCode()
	{
		return Certificate.GetHashCode();
	}
}
