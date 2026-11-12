namespace Microsoft.AspNetCore.Routing;

public interface IRouteCollection : IRouter
{
	void Add(IRouter router);
}
