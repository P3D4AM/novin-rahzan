using System;
using System.Runtime.Serialization;

namespace mCore;

public class WapPushException : Exception
{
	public WapPushException()
	{
	}

	public WapPushException(string message)
		: base(message)
	{
	}

	public WapPushException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public WapPushException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
