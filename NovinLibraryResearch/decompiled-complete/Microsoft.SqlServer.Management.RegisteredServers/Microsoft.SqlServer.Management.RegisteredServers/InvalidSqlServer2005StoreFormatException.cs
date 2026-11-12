using System;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.RegisteredServers;

[Serializable]
public class InvalidSqlServer2005StoreFormatException : RegisteredServerException
{
	public InvalidSqlServer2005StoreFormatException()
	{
		Init();
	}

	public InvalidSqlServer2005StoreFormatException(string message)
		: base(message)
	{
		Init();
	}

	public InvalidSqlServer2005StoreFormatException(string message, Exception innerException)
		: base(message, innerException)
	{
		Init();
	}

	protected InvalidSqlServer2005StoreFormatException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
