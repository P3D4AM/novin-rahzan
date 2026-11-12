namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationResult
{
	public bool Succeeded { get; private set; }

	public AuthorizationFailure Failure { get; private set; }

	private AuthorizationResult()
	{
	}

	public static AuthorizationResult Success()
	{
		return new AuthorizationResult
		{
			Succeeded = true
		};
	}

	public static AuthorizationResult Failed(AuthorizationFailure failure)
	{
		return new AuthorizationResult
		{
			Failure = failure
		};
	}

	public static AuthorizationResult Failed()
	{
		return new AuthorizationResult
		{
			Failure = AuthorizationFailure.ExplicitFail()
		};
	}
}
