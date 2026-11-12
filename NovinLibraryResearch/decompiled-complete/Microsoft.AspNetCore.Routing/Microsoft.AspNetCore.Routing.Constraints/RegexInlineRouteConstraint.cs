namespace Microsoft.AspNetCore.Routing.Constraints;

public class RegexInlineRouteConstraint : RegexRouteConstraint
{
	public RegexInlineRouteConstraint(string regexPattern)
		: base(regexPattern)
	{
	}
}
