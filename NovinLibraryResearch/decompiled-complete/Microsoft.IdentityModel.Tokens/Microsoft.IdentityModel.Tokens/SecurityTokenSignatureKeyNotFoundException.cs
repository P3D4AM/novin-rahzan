using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenSignatureKeyNotFoundException : SecurityTokenInvalidSignatureException
{
	public SecurityTokenSignatureKeyNotFoundException()
	{
	}

	public SecurityTokenSignatureKeyNotFoundException(string message)
		: base(message)
	{
	}

	public SecurityTokenSignatureKeyNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenSignatureKeyNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
