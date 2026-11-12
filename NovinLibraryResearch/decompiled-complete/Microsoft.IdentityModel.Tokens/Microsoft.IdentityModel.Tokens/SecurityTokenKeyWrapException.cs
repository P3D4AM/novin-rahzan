using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenKeyWrapException : SecurityTokenException
{
	public SecurityTokenKeyWrapException()
	{
	}

	public SecurityTokenKeyWrapException(string message)
		: base(message)
	{
	}

	public SecurityTokenKeyWrapException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenKeyWrapException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
