using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenReplayDetectedException : SecurityTokenValidationException
{
	public SecurityTokenReplayDetectedException()
		: base("SecurityToken replay detected")
	{
	}

	public SecurityTokenReplayDetectedException(string message)
		: base(message)
	{
	}

	public SecurityTokenReplayDetectedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityTokenReplayDetectedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
