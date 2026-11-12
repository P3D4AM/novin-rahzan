namespace Microsoft.AspNetCore.Authorization;

public interface IAuthorizationEvaluator
{
	AuthorizationResult Evaluate(AuthorizationHandlerContext context);
}
