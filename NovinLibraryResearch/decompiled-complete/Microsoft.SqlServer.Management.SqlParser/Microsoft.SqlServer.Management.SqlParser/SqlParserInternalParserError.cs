using System;

namespace Microsoft.SqlServer.Management.SqlParser;

[Serializable]
public class SqlParserInternalParserError : SqlParserException
{
	internal SqlParserInternalParserError(Exception innerException)
		: base("Internal Parser Error", innerException)
	{
	}
}
