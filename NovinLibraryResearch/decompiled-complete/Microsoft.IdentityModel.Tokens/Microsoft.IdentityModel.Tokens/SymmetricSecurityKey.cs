using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class SymmetricSecurityKey : SecurityKey
{
	private int _keySize;

	private byte[] _key;

	public override int KeySize => _keySize;

	public virtual byte[] Key => _key.CloneByteArray();

	internal SymmetricSecurityKey(JsonWebKey webKey)
		: base(webKey)
	{
		if (string.IsNullOrEmpty(webKey.K))
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10703: Cannot create a '{0}', key length is zero.", typeof(SymmetricSecurityKey))));
		}
		_key = Base64UrlEncoder.DecodeBytes(webKey.K);
		_keySize = _key.Length * 8;
		webKey.ConvertedSecurityKey = this;
	}

	public SymmetricSecurityKey(byte[] key)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (key.Length == 0)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10703: Cannot create a '{0}', key length is zero.", typeof(SymmetricSecurityKey))));
		}
		_key = key.CloneByteArray();
		_keySize = _key.Length * 8;
	}

	public override bool CanComputeJwkThumbprint()
	{
		return true;
	}

	public override byte[] ComputeJwkThumbprint()
	{
		return Utility.GenerateSha256Hash("{\"k\":\"" + Base64UrlEncoder.Encode(Key) + "\",\"kty\":\"oct\"}");
	}
}
