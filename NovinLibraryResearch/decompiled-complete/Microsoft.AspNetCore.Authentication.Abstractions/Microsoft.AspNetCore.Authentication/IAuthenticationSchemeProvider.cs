using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationSchemeProvider
{
	Task<IEnumerable<AuthenticationScheme>> GetAllSchemesAsync();

	Task<AuthenticationScheme> GetSchemeAsync(string name);

	Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync();

	Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync();

	Task<AuthenticationScheme> GetDefaultForbidSchemeAsync();

	Task<AuthenticationScheme> GetDefaultSignInSchemeAsync();

	Task<AuthenticationScheme> GetDefaultSignOutSchemeAsync();

	void AddScheme(AuthenticationScheme scheme);

	void RemoveScheme(string name);

	Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync();
}
