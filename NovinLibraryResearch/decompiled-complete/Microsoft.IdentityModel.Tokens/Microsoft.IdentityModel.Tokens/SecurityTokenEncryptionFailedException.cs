using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenEncryptionFailedException : SecurityTokenException
{
	public SecurityTokenEncryptionFailedException()
	{
	}

	public SecurityTokenEncryptionFailedException(string message)
		: base(message)
	{
	}

	public SecurityTokenEncryptionFailedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenEncryptionFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
