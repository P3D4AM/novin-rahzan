using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenReplayAddFailedException : SecurityTokenValidationException
{
	public SecurityTokenReplayAddFailedException()
	{
	}

	public SecurityTokenReplayAddFailedException(string message)
		: base(message)
	{
	}

	public SecurityTokenReplayAddFailedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenReplayAddFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
