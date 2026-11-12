using System;

namespace Microsoft.Data.SqlClient;

internal class AlwaysEncryptedAttestationException : Exception
{
	public AlwaysEncryptedAttestationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public AlwaysEncryptedAttestationException(string message)
		: base(message)
	{
	}

	public AlwaysEncryptedAttestationException()
	{
	}
}
