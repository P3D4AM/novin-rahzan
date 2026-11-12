using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class ECDsaSecurityKey : AsymmetricSecurityKey
{
	private bool? _hasPrivateKey;

	public ECDsa ECDsa { get; private set; }

	[Obsolete("HasPrivateKey method is deprecated, please use FoundPrivateKey instead.")]
	public override bool HasPrivateKey
	{
		get
		{
			if (!_hasPrivateKey.HasValue)
			{
				try
				{
					ECDsa.SignHash(new byte[20]);
					_hasPrivateKey = true;
				}
				catch (CryptographicException)
				{
					_hasPrivateKey = false;
				}
			}
			return _hasPrivateKey.Value;
		}
	}

	public override PrivateKeyStatus PrivateKeyStatus => PrivateKeyStatus.Unknown;

	public override int KeySize => ECDsa.KeySize;

	internal ECDsaSecurityKey(JsonWebKey webKey, bool usePrivateKey)
		: base(webKey)
	{
		ECDsa = ECDsaAdapter.Instance.CreateECDsa(webKey, usePrivateKey);
		webKey.ConvertedSecurityKey = this;
	}

	public ECDsaSecurityKey(ECDsa ecdsa)
	{
		ECDsa = ecdsa ?? throw LogHelper.LogArgumentNullException("ecdsa");
	}

	public override bool CanComputeJwkThumbprint()
	{
		return false;
	}

	public override byte[] ComputeJwkThumbprint()
	{
		throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException("IDX10695: Unable to create a JsonWebKey from an ECDsa object. Required ECParameters structure is not supported by .NET Framework < 4.7."));
	}
}
