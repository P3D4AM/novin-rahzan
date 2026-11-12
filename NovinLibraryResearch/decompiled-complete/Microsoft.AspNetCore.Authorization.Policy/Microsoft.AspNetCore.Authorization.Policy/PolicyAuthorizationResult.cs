namespace Microsoft.AspNetCore.Authorization.Policy;

public class PolicyAuthorizationResult
{
	public bool Challenged { get; private set; }

	public bool Forbidden { get; private set; }

	public bool Succeeded { get; private set; }

	private PolicyAuthorizationResult()
	{
	}

	public static PolicyAuthorizationResult Challenge()
	{
		return new PolicyAuthorizationResult
		{
			Challenged = true
		};
	}

	public static PolicyAuthorizationResult Forbid()
	{
		return new PolicyAuthorizationResult
		{
			Forbidden = true
		};
	}

	public static PolicyAuthorizationResult Success()
	{
		return new PolicyAuthorizationResult
		{
			Succeeded = true
		};
	}
}
