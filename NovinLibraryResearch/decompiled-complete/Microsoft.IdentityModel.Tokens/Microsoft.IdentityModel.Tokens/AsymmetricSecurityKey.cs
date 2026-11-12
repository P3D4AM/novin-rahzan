using System;

namespace Microsoft.IdentityModel.Tokens;

public abstract class AsymmetricSecurityKey : SecurityKey
{
	[Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus instead.")]
	public abstract bool HasPrivateKey { get; }

	public abstract PrivateKeyStatus PrivateKeyStatus { get; }

	public AsymmetricSecurityKey()
	{
	}

	internal AsymmetricSecurityKey(SecurityKey key)
		: base(key)
	{
	}
}
