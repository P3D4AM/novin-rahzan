using System;
using System.Runtime.Serialization;

namespace mCore;

public class SMSReadException : Exception
{
	public SMSReadException()
	{
	}

	public SMSReadException(string message)
		: base(message)
	{
	}

	public SMSReadException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public SMSReadException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
