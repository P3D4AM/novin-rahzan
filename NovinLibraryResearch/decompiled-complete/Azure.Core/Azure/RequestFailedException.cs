using System;
using System.Runtime.Serialization;
using Azure.Core;

namespace Azure;

[Serializable]
public class RequestFailedException : Exception, ISerializable
{
	public int Status { get; }

	public string? ErrorCode { get; }

	public RequestFailedException(string message)
		: this(0, message)
	{
	}

	public RequestFailedException(string message, Exception? innerException)
		: this(0, message, innerException)
	{
	}

	public RequestFailedException(int status, string message)
		: this(status, message, null)
	{
	}

	public RequestFailedException(int status, string message, Exception? innerException)
		: this(status, message, null, innerException)
	{
	}

	public RequestFailedException(int status, string message, string? errorCode, Exception? innerException)
		: base(message, innerException)
	{
		Status = status;
		ErrorCode = errorCode;
	}

	protected RequestFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		Status = info.GetInt32("Status");
		ErrorCode = info.GetString("ErrorCode");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		Argument.AssertNotNull(info, "info");
		info.AddValue("Status", Status);
		info.AddValue("ErrorCode", ErrorCode);
		base.GetObjectData(info, context);
	}
}
