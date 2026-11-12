using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.Internal.Requests.Silent;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal class ClientApplicationBaseExecutor : AbstractExecutor, IClientApplicationBaseExecutor
{
	private readonly ClientApplicationBase _clientApplicationBase;

	public ClientApplicationBaseExecutor(IServiceBundle serviceBundle, ClientApplicationBase clientApplicationBase)
		: base(serviceBundle, clientApplicationBase)
	{
		_clientApplicationBase = clientApplicationBase;
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenSilentParameters silentParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _clientApplicationBase.CreateRequestParameters(commonParameters, requestContext, _clientApplicationBase.UserTokenCacheInternal);
		authenticationRequestParameters.SendX5C = silentParameters.SendX5C;
		return await new SilentRequest(base.ServiceBundle, authenticationRequestParameters, silentParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByRefreshTokenParameters refreshTokenParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		if (commonParameters.Scopes == null || !commonParameters.Scopes.Any())
		{
			commonParameters.Scopes = new SortedSet<string> { _clientApplicationBase.AppConfig.ClientId + "/.default" };
			requestContext.Logger.Info("No scopes provided for acquire token by refresh token request. Using default scope instead.");
		}
		AuthenticationRequestParameters authenticationRequestParameters = _clientApplicationBase.CreateRequestParameters(commonParameters, requestContext, _clientApplicationBase.UserTokenCacheInternal);
		requestContext.Logger.Info(LogMessages.UsingXScopesForRefreshTokenRequest(commonParameters.Scopes.Count()));
		authenticationRequestParameters.SendX5C = refreshTokenParameters.SendX5C;
		return await new ByRefreshTokenRequest(base.ServiceBundle, authenticationRequestParameters, refreshTokenParameters).RunAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
	}
}
