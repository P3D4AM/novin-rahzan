using System.Collections.Generic;

namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationFailure
{
	public bool FailCalled { get; private set; }

	public IEnumerable<IAuthorizationRequirement> FailedRequirements { get; private set; }

	private AuthorizationFailure()
	{
	}

	public static AuthorizationFailure ExplicitFail()
	{
		return new AuthorizationFailure
		{
			FailCalled = true,
			FailedRequirements = new IAuthorizationRequirement[0]
		};
	}

	public static AuthorizationFailure Failed(IEnumerable<IAuthorizationRequirement> failed)
	{
		return new AuthorizationFailure
		{
			FailedRequirements = failed
		};
	}
}
