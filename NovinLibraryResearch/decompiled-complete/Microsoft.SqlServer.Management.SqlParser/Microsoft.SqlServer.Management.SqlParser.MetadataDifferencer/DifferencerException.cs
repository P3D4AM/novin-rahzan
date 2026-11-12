using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

[Serializable]
public class DifferencerException : Exception
{
	internal DifferencerException()
	{
	}

	internal DifferencerException(string message)
		: base(message)
	{
	}

	internal DifferencerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
