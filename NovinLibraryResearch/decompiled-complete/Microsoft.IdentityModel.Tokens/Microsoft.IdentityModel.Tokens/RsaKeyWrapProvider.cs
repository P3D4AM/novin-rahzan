using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class RsaKeyWrapProvider : KeyWrapProvider
{
	private AsymmetricAdapter _asymmetricAdapter;

	private bool _disposed;

	public override string Algorithm { get; }

	public override string Context { get; set; }

	public override SecurityKey Key { get; }

	public RsaKeyWrapProvider(SecurityKey key, string algorithm, bool willUnwrap)
	{
		if (key == null)
		{
			throw LogHelper.LogArgumentNullException("key");
		}
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (!IsSupportedAlgorithm(key, algorithm))
		{
			throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10661: Unable to create the KeyWrapProvider.\nKeyWrapAlgorithm: '{0}', SecurityKey: '{1}'\n is not supported.", algorithm, key)));
		}
		Algorithm = algorithm;
		Key = key;
		_asymmetricAdapter = new AsymmetricAdapter(key, algorithm, willUnwrap);
	}

	protected override void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_disposed = true;
			_asymmetricAdapter.Dispose();
		}
	}

	protected virtual bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
	{
		return SupportedAlgorithms.IsSupportedRsaKeyWrap(algorithm, key);
	}

	public override byte[] UnwrapKey(byte[] keyBytes)
	{
		if (keyBytes == null || keyBytes.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("keyBytes");
		}
		if (_disposed)
		{
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		try
		{
			return _asymmetricAdapter.Decrypt(keyBytes);
		}
		catch (Exception ex)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant("IDX10659: UnwrapKey failed, exception from cryptographic operation: '{0}'", ex)));
		}
	}

	public override byte[] WrapKey(byte[] keyBytes)
	{
		if (keyBytes == null || keyBytes.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("keyBytes");
		}
		if (_disposed)
		{
			throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
		}
		try
		{
			return _asymmetricAdapter.Encrypt(keyBytes);
		}
		catch (Exception ex)
		{
			throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant("IDX10658: WrapKey failed, exception from cryptographic operation: '{0}'", ex)));
		}
	}
}
