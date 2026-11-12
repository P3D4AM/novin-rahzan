using System;

namespace Microsoft.IdentityModel.Tokens;

public abstract class SecurityToken
{
	public abstract string Id { get; }

	public abstract string Issuer { get; }

	public abstract SecurityKey SecurityKey { get; }

	public abstract SecurityKey SigningKey { get; set; }

	public abstract DateTime ValidFrom { get; }

	public abstract DateTime ValidTo { get; }
}
