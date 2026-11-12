using System;
using System.Runtime.Serialization;

namespace mCore;

public class vCalendarException : Exception
{
	public vCalendarException()
	{
	}

	public vCalendarException(string message)
		: base(message)
	{
	}

	public vCalendarException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public vCalendarException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
