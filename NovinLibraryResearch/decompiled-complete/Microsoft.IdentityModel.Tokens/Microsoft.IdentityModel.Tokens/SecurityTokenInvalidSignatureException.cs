using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenInvalidSignatureException : SecurityTokenValidationException
{
	public SecurityTokenInvalidSignatureException()
	{
	}

	public SecurityTokenInvalidSignatureException(string message)
		: base(message)
	{
	}

	public SecurityTokenInvalidSignatureException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenInvalidSignatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
