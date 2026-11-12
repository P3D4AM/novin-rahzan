using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Broker;

internal class NullBroker : IBroker
{
	public bool IsBrokerInstalledAndInvokable()
	{
		return false;
	}

	public Task<MsalTokenResponse> AcquireTokenInteractiveAsync(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenInteractiveParameters acquireTokenInteractiveParameters)
	{
		throw new NotImplementedException();
	}

	public Task<MsalTokenResponse> AcquireTokenSilentAsync(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters acquireTokenSilentParameters)
	{
		throw new NotImplementedException();
	}

	public void HandleInstallUrl(string appLink)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<IAccount>> GetAccountsAsync(string clientID, string redirectUri)
	{
		throw new NotImplementedException();
	}

	public Task RemoveAccountAsync(IApplicationConfiguration appConfig, IAccount account)
	{
		throw new NotImplementedException();
	}

	public Task<MsalTokenResponse> AcquireTokenSilentDefaultUserAsync(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters acquireTokenSilentParameters)
	{
		throw new NotImplementedException();
	}
}
