using System;

namespace Brotli;

public class BrotliException : Exception
{
	public BrotliException()
	{
	}

	public BrotliException(string message)
		: base(message)
	{
	}

	public BrotliException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
