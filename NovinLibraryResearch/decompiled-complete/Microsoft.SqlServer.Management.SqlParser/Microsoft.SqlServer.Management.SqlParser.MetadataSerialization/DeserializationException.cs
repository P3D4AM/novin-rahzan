using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

[Serializable]
internal class DeserializationException : Exception
{
	internal DeserializationException()
	{
	}

	internal DeserializationException(string message)
		: base(message)
	{
	}

	internal DeserializationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
