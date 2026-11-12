using System;

namespace Azure.Identity;

public class AuthenticationFailedException : Exception
{
	public AuthenticationFailedException(string message)
		: this(message, null)
	{
	}

	public AuthenticationFailedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
