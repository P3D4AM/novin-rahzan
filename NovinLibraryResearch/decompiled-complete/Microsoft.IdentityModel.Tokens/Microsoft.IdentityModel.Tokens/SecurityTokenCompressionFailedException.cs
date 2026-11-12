using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenCompressionFailedException : SecurityTokenException
{
	public SecurityTokenCompressionFailedException()
		: base("SecurityToken compression failed.")
	{
	}

	public SecurityTokenCompressionFailedException(string message)
		: base(message)
	{
	}

	public SecurityTokenCompressionFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityTokenCompressionFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
