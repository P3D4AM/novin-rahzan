using System;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authentication;

public class AuthenticateResult
{
	public bool Succeeded => Ticket != null;

	public AuthenticationTicket Ticket { get; protected set; }

	public ClaimsPrincipal Principal => Ticket?.Principal;

	public AuthenticationProperties Properties { get; protected set; }

	public Exception Failure { get; protected set; }

	public bool None { get; protected set; }

	protected AuthenticateResult()
	{
	}

	public static AuthenticateResult Success(AuthenticationTicket ticket)
	{
		if (ticket == null)
		{
			throw new ArgumentNullException("ticket");
		}
		return new AuthenticateResult
		{
			Ticket = ticket,
			Properties = ticket.Properties
		};
	}

	public static AuthenticateResult NoResult()
	{
		return new AuthenticateResult
		{
			None = true
		};
	}

	public static AuthenticateResult Fail(Exception failure)
	{
		return new AuthenticateResult
		{
			Failure = failure
		};
	}

	public static AuthenticateResult Fail(Exception failure, AuthenticationProperties properties)
	{
		return new AuthenticateResult
		{
			Failure = failure,
			Properties = properties
		};
	}

	public static AuthenticateResult Fail(string failureMessage)
	{
		return Fail(new Exception(failureMessage));
	}

	public static AuthenticateResult Fail(string failureMessage, AuthenticationProperties properties)
	{
		return Fail(new Exception(failureMessage), properties);
	}
}
