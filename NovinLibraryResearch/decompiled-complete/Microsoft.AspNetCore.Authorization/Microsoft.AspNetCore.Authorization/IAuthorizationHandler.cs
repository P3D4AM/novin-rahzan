using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public interface IAuthorizationHandler
{
	Task HandleAsync(AuthorizationHandlerContext context);
}
