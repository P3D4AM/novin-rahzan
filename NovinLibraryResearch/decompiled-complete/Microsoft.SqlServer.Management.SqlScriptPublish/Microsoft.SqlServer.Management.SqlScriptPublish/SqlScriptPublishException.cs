using System;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public class SqlScriptPublishException : Exception
{
	public SqlScriptPublishException(string message)
		: base(message)
	{
	}

	public SqlScriptPublishException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
