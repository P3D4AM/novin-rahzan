using System;
using System.IO;

namespace Azure.Core;

public class ResponseClassifier
{
	public virtual bool IsRetriableResponse(HttpMessage message)
	{
		switch (message.Response.Status)
		{
		case 408:
		case 429:
		case 500:
		case 502:
		case 503:
		case 504:
			return true;
		default:
			return false;
		}
	}

	public virtual bool IsRetriableException(Exception exception)
	{
		if (!(exception is IOException))
		{
			if (exception is RequestFailedException ex)
			{
				return ex.Status == 0;
			}
			return false;
		}
		return true;
	}

	public virtual bool IsRetriable(HttpMessage message, Exception exception)
	{
		if (!IsRetriableException(exception))
		{
			if (exception is OperationCanceledException)
			{
				return !message.CancellationToken.IsCancellationRequested;
			}
			return false;
		}
		return true;
	}

	public virtual bool IsErrorResponse(HttpMessage message)
	{
		int num = message.Response.Status / 100;
		if (num != 4)
		{
			return num == 5;
		}
		return true;
	}
}
