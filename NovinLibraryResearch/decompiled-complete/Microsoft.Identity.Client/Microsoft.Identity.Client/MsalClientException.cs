using System;

namespace Microsoft.Identity.Client;

public class MsalClientException : MsalException
{
	public MsalClientException(string errorCode)
		: base(errorCode)
	{
	}

	public MsalClientException(string errorCode, string errorMessage)
		: base(errorCode, errorMessage)
	{
	}

	public MsalClientException(string errorCode, string errorMessage, Exception innerException)
		: base(errorCode, errorMessage, innerException)
	{
	}
}
