using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationHandlerProvider
{
	Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme);
}
