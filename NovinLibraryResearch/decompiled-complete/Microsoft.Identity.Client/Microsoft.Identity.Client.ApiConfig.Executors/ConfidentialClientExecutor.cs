using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal class ConfidentialClientExecutor : AbstractExecutor, IConfidentialClientApplicationExecutor
{
	private readonly ConfidentialClientApplication _confidentialClientApplication;

	public ConfidentialClientExecutor(IServiceBundle serviceBundle, ConfidentialClientApplication confidentialClientApplication)
		: base(serviceBundle, confidentialClientApplication)
	{
		_confidentialClientApplication = confidentialClientApplication;
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByAuthorizationCodeParameters authorizationCodeParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _confidentialClientApplication.CreateRequestParameters(commonParameters, requestContext, _confidentialClientApplication.UserTokenCacheInternal);
		authenticationRequestParameters.SendX5C = authorizationCodeParameters.SendX5C;
		return await new ConfidentialAuthCodeRequest(base.ServiceBundle, authenticationRequestParameters, authorizationCodeParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenForClientParameters clientParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _confidentialClientApplication.CreateRequestParameters(commonParameters, requestContext, _confidentialClientApplication.AppTokenCacheInternal);
		if (clientParameters.AutoDetectRegion && requestContext.ServiceBundle.Config.AuthorityInfo.AuthorityType == AuthorityType.Adfs)
		{
			throw new MsalClientException("region_discovery_unavailable", "Region discovery cannot be performed for ADFS authority. Do not set WithAzureRegion to true.");
		}
		authenticationRequestParameters.SendX5C = clientParameters.SendX5C;
		requestContext.ServiceBundle.Config.AuthorityInfo.AutoDetectRegion = clientParameters.AutoDetectRegion;
		return await new ClientCredentialRequest(base.ServiceBundle, authenticationRequestParameters, clientParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenOnBehalfOfParameters onBehalfOfParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters authenticationRequestParameters = _confidentialClientApplication.CreateRequestParameters(commonParameters, requestContext, _confidentialClientApplication.UserTokenCacheInternal);
		authenticationRequestParameters.SendX5C = onBehalfOfParameters.SendX5C;
		authenticationRequestParameters.UserAssertion = onBehalfOfParameters.UserAssertion;
		return await new OnBehalfOfRequest(base.ServiceBundle, authenticationRequestParameters, onBehalfOfParameters).RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Uri> ExecuteAsync(AcquireTokenCommonParameters commonParameters, GetAuthorizationRequestUrlParameters authorizationRequestUrlParameters, CancellationToken cancellationToken)
	{
		RequestContext requestContext = CreateRequestContextAndLogVersionInfo(commonParameters.CorrelationId);
		AuthenticationRequestParameters requestParameters = _confidentialClientApplication.CreateRequestParameters(commonParameters, requestContext, _confidentialClientApplication.UserTokenCacheInternal);
		requestParameters.Account = authorizationRequestUrlParameters.Account;
		requestParameters.LoginHint = authorizationRequestUrlParameters.LoginHint;
		if (!string.IsNullOrWhiteSpace(authorizationRequestUrlParameters.RedirectUri))
		{
			requestParameters.RedirectUri = new Uri(authorizationRequestUrlParameters.RedirectUri);
		}
		await AuthorityEndpoints.UpdateAuthorityEndpointsAsync(requestParameters).ConfigureAwait(continueOnCapturedContext: false);
		return new AuthCodeRequestComponent(requestParameters, authorizationRequestUrlParameters.ToInteractiveParameters()).GetAuthorizationUriWithoutPkce();
	}
}
