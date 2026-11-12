using System;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

[Serializable]
public class RegisteredServerException : ApplicationException
{
	public RegisteredServerException(string message, Exception previous)
		: base(message, previous)
	{
	}

	public RegisteredServerException(string message)
		: base(message)
	{
	}

	public RegisteredServerException()
	{
	}

	protected RegisteredServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
