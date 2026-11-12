namespace Microsoft.AspNetCore.Routing.Constraints;

public class AlphaRouteConstraint : RegexRouteConstraint
{
	public AlphaRouteConstraint()
		: base("^[a-z]*$")
	{
	}
}
