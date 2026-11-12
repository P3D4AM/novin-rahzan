using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public abstract class SignatureProvider : IDisposable
{
	public string Algorithm { get; private set; }

	public string Context { get; set; }

	public CryptoProviderCache CryptoProviderCache { get; set; }

	public SecurityKey Key { get; private set; }

	internal virtual int ObjectPoolSize => 0;

	public bool WillCreateSignatures { get; protected set; }

	protected SignatureProvider(SecurityKey key, string algorithm)
	{
		Key = key ?? throw LogHelper.LogArgumentNullException("key");
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		Algorithm = algorithm;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected abstract void Dispose(bool disposing);

	public abstract byte[] Sign(byte[] input);

	public abstract bool Verify(byte[] input, byte[] signature);
}
