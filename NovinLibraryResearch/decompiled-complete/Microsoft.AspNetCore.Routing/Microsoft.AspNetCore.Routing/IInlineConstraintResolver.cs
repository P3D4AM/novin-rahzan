namespace Microsoft.AspNetCore.Routing;

public interface IInlineConstraintResolver
{
	IRouteConstraint ResolveConstraint(string inlineConstraint);
}
