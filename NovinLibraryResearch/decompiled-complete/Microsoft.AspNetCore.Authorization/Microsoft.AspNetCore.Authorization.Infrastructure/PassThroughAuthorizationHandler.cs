using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization.Infrastructure;

public class PassThroughAuthorizationHandler : IAuthorizationHandler
{
	public async Task HandleAsync(AuthorizationHandlerContext context)
	{
		foreach (IAuthorizationHandler item in context.Requirements.OfType<IAuthorizationHandler>())
		{
			await item.HandleAsync(context);
		}
	}
}
