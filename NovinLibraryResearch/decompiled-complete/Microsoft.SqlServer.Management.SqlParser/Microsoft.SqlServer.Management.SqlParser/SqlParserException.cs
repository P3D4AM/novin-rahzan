using System;

namespace Microsoft.SqlServer.Management.SqlParser;

[Serializable]
public class SqlParserException : Exception
{
	internal SqlParserException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
