using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Template;

namespace Microsoft.AspNetCore.Routing.Tree;

public class OutboundRouteEntry
{
	public IDictionary<string, IRouteConstraint> Constraints { get; set; }

	public RouteValueDictionary Defaults { get; set; }

	public IRouter Handler { get; set; }

	public int Order { get; set; }

	public decimal Precedence { get; set; }

	public string RouteName { get; set; }

	public RouteValueDictionary RequiredLinkValues { get; set; }

	public RouteTemplate RouteTemplate { get; set; }
}
