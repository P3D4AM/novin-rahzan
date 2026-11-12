using System;
using Azure.Core;

namespace Azure.Identity;

internal class AuthenticationRequiredException : CredentialUnavailableException
{
	public TokenRequestContext TokenRequestContext { get; }

	public AuthenticationRequiredException(string message, TokenRequestContext context)
		: this(message, context, null)
	{
	}

	public AuthenticationRequiredException(string message, TokenRequestContext context, Exception innerException)
		: base(message, innerException)
	{
		TokenRequestContext = context;
	}
}
