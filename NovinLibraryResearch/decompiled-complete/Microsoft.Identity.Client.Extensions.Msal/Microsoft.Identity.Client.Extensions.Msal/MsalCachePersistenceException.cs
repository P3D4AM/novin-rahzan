using System;
using System.Runtime.Serialization;

namespace Microsoft.Identity.Client.Extensions.Msal;

public class MsalCachePersistenceException : Exception
{
	public MsalCachePersistenceException()
	{
	}

	public MsalCachePersistenceException(string message)
		: base(message)
	{
	}

	public MsalCachePersistenceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MsalCachePersistenceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
