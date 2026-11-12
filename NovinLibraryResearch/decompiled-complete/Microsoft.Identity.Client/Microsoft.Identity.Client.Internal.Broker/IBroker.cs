using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Broker;

internal interface IBroker
{
	bool IsBrokerInstalledAndInvokable();

	Task<MsalTokenResponse> AcquireTokenInteractiveAsync(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenInteractiveParameters acquireTokenInteractiveParameters);

	Task<MsalTokenResponse> AcquireTokenSilentAsync(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters acquireTokenSilentParameters);

	Task<MsalTokenResponse> AcquireTokenSilentDefaultUserAsync(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters acquireTokenSilentParameters);

	void HandleInstallUrl(string appLink);

	Task<IEnumerable<IAccount>> GetAccountsAsync(string clientID, string redirectUri);

	Task RemoveAccountAsync(IApplicationConfiguration appConfig, IAccount account);
}
