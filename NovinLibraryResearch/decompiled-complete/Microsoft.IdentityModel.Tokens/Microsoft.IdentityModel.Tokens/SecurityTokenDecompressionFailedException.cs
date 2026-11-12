using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenDecompressionFailedException : SecurityTokenException
{
	public SecurityTokenDecompressionFailedException()
		: base("SecurityToken decompression failed.")
	{
	}

	public SecurityTokenDecompressionFailedException(string message)
		: base(message)
	{
	}

	public SecurityTokenDecompressionFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityTokenDecompressionFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
