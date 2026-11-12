using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationRequestHandler : IAuthenticationHandler
{
	Task<bool> HandleRequestAsync();
}
