namespace Microsoft.AspNetCore.Routing;

public interface IRoutingFeature
{
	RouteData RouteData { get; set; }
}
