namespace Microsoft.AspNetCore.Authorization;

public class DefaultAuthorizationEvaluator : IAuthorizationEvaluator
{
	public AuthorizationResult Evaluate(AuthorizationHandlerContext context)
	{
		if (!context.HasSucceeded)
		{
			return AuthorizationResult.Failed(context.HasFailed ? AuthorizationFailure.ExplicitFail() : AuthorizationFailure.Failed(context.PendingRequirements));
		}
		return AuthorizationResult.Success();
	}
}
