using System;
using System.Runtime.Serialization;

namespace mCore;

public class SMSSendException : Exception
{
	public SMSSendException()
	{
	}

	public SMSSendException(string message)
		: base(message)
	{
	}

	public SMSSendException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public SMSSendException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
