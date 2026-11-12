using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Routing;

public interface IRouter
{
	Task RouteAsync(RouteContext context);

	VirtualPathData GetVirtualPath(VirtualPathContext context);
}
