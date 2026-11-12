using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal class PublicClientExecutor : AbstractExecutor, IPublicClientApplicationExecutor
{
	private readonly PublicClientApplication _publicClientApplication;

	public PublicClientExecutor(IServiceBundle serviceBundle, PublicClientApplication publicClientApplication)
		: base(serviceBundle, publicClientApplication)
	{
		_publicClientApplication = publicClientApplication;
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenInteractiveParameters interactiveParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _publicClientApplication.CreateRequestParameters(commonParameters, requestContext, _publicClientApplication.UserTokenCacheInternal);
		authenticationRequestParameters.LoginHint = interactiveParameters.LoginHint;
		authenticationRequestParameters.Account = interactiveParameters.Account;
		return await new InteractiveRequest(authenticationRequestParameters, interactiveParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenWithDeviceCodeParameters deviceCodeParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _publicClientApplication.CreateRequestParameters(commonParameters, requestContext, _publicClientApplication.UserTokenCacheInternal);
		return await new DeviceCodeRequest(base.ServiceBundle, authenticationRequestParameters, deviceCodeParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByIntegratedWindowsAuthParameters integratedWindowsAuthParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _publicClientApplication.CreateRequestParameters(commonParameters, requestContext, _publicClientApplication.UserTokenCacheInternal);
		return await new IntegratedWindowsAuthRequest(base.ServiceBundle, authenticationRequestParameters, integratedWindowsAuthParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByUsernamePasswordParameters usernamePasswordParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _publicClientApplication.CreateRequestParameters(commonParameters, requestContext, _publicClientApplication.UserTokenCacheInternal);
		return await new UsernamePasswordRequest(base.ServiceBundle, authenticationRequestParameters, usernamePasswordParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}
}
