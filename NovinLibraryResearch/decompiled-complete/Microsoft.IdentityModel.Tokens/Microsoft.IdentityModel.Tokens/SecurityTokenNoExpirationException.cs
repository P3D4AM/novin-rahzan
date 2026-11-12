using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenNoExpirationException : SecurityTokenValidationException
{
	public SecurityTokenNoExpirationException()
	{
	}

	public SecurityTokenNoExpirationException(string message)
		: base(message)
	{
	}

	public SecurityTokenNoExpirationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenNoExpirationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
