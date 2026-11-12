using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class InvalidXmlException : DeserializationException
{
	internal InvalidXmlException()
	{
	}

	internal InvalidXmlException(string message)
		: base(message)
	{
	}

	internal InvalidXmlException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
