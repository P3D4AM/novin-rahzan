using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenValidationException : SecurityTokenException
{
	public SecurityTokenValidationException()
	{
	}

	public SecurityTokenValidationException(string message)
		: base(message)
	{
	}

	public SecurityTokenValidationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenValidationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
