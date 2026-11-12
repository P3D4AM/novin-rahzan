using System;
using System.Runtime.Serialization;

namespace ExcelDataReader.Exceptions;

[Serializable]
public class HeaderException : ExcelReaderException
{
	public HeaderException()
	{
	}

	public HeaderException(string message)
		: base(message)
	{
	}

	public HeaderException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HeaderException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
