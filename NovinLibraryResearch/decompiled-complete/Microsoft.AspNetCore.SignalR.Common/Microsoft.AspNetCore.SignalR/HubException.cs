using System;
using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.SignalR;

[Serializable]
public class HubException : Exception
{
	public HubException()
	{
	}

	public HubException(string? message)
		: base(message)
	{
	}

	public HubException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public HubException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
