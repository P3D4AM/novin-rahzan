using System;
using System.Runtime.Serialization;

namespace mCore;

public class SMSDeleteException : Exception
{
	public SMSDeleteException()
	{
	}

	public SMSDeleteException(string message)
		: base(message)
	{
	}

	public SMSDeleteException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public SMSDeleteException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
