using System;

namespace Microsoft.IdentityModel.Tokens;

public abstract class KeyWrapProvider : IDisposable
{
	public abstract string Algorithm { get; }

	public abstract string Context { get; set; }

	public abstract SecurityKey Key { get; }

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected abstract void Dispose(bool disposing);

	public abstract byte[] UnwrapKey(byte[] keyBytes);

	public abstract byte[] WrapKey(byte[] keyBytes);
}
