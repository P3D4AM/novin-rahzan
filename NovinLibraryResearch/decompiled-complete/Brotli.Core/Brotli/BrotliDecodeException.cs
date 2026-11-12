using System;

namespace Brotli;

public class BrotliDecodeException : BrotliException
{
	public int Code { get; set; }

	public string ErrorText { get; set; }

	public BrotliDecodeException(int code, string errorText)
	{
		Code = code;
		ErrorText = errorText;
	}

	public BrotliDecodeException(string message, int code, string errorText)
		: base(message)
	{
		Code = code;
		ErrorText = errorText;
	}

	public BrotliDecodeException(string message, Exception innerException, int code, string errorText)
		: base(message, innerException)
	{
		Code = code;
		ErrorText = errorText;
	}
}
