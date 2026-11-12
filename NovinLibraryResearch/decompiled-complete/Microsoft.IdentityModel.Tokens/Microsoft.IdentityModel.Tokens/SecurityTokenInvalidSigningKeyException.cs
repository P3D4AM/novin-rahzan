using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidSigningKeyException : SecurityTokenValidationException
{
	public SecurityKey SigningKey { get; set; }

	public SecurityTokenInvalidSigningKeyException()
		: base("SecurityToken has invalid issuer signing key.")
	{
	}

	public SecurityTokenInvalidSigningKeyException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidSigningKeyException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityTokenInvalidSigningKeyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
