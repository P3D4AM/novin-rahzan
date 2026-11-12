using System;
using System.Runtime.Serialization;

namespace mCore;

public class GeneralException : Exception
{
	public GeneralException()
	{
	}

	public GeneralException(string message)
		: base(message)
	{
	}

	public GeneralException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public GeneralException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
