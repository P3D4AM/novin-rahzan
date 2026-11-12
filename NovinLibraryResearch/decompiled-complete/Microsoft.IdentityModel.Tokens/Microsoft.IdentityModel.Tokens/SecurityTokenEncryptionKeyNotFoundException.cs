using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens;

[Serializable]
public class SecurityTokenEncryptionKeyNotFoundException : SecurityTokenDecryptionFailedException
{
	public SecurityTokenEncryptionKeyNotFoundException()
	{
	}

	public SecurityTokenEncryptionKeyNotFoundException(string message)
		: base(message)
	{
	}

	public SecurityTokenEncryptionKeyNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecurityTokenEncryptionKeyNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
