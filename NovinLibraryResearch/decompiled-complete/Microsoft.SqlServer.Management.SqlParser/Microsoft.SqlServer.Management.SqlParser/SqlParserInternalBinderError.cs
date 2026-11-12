using System;

namespace Microsoft.SqlServer.Management.SqlParser;

[Serializable]
public class SqlParserInternalBinderError : SqlParserException
{
	internal SqlParserInternalBinderError(Exception innerException)
		: base("Internal Binder Error", innerException)
	{
	}
}
