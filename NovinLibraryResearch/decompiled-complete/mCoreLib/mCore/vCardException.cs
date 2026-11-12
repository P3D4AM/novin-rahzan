using System;
using System.Runtime.Serialization;

namespace mCore;

public class vCardException : Exception
{
	public vCardException()
	{
	}

	public vCardException(string message)
		: base(message)
	{
	}

	public vCardException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public vCardException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
