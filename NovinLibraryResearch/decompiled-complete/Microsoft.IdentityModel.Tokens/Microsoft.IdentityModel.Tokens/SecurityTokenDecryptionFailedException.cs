using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenDecryptionFailedException : SecurityTokenException
{
	public SecurityTokenDecryptionFailedException()
	{
	}

	public SecurityTokenDecryptionFailedException(string message)
		: base(message)
	{
	}

	public SecurityTokenDecryptionFailedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenDecryptionFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
