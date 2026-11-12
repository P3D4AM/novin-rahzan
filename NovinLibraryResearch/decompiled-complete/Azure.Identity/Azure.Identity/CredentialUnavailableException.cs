using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Identity;

public class CredentialUnavailableException : AuthenticationFailedException
{
	public CredentialUnavailableException(string message)
		: this(message, null)
	{
	}

	public CredentialUnavailableException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	internal static CredentialUnavailableException CreateAggregateException(string message, IList<CredentialUnavailableException> exceptions)
	{
		if (exceptions.Count == 1)
		{
			return exceptions[0];
		}
		StringBuilder stringBuilder = new StringBuilder(message);
		foreach (CredentialUnavailableException exception in exceptions)
		{
			stringBuilder.Append(Environment.NewLine).Append("- ").Append(exception.Message);
		}
		AggregateException innerException = new AggregateException("Multiple exceptions were encountered while attempting to authenticate.", exceptions);
		return new CredentialUnavailableException(stringBuilder.ToString(), innerException);
	}
}
