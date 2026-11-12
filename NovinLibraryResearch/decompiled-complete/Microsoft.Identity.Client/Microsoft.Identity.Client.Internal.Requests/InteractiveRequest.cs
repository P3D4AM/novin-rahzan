using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Broker;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class InteractiveRequest : RequestBase
{
	private readonly AuthenticationRequestParameters _requestParams;

	private readonly AcquireTokenInteractiveParameters _interactiveParameters;

	private readonly IServiceBundle _serviceBundle;

	private readonly ICoreLogger _logger;

	private readonly IAuthCodeRequestComponent _authCodeRequestComponentOverride;

	private readonly ITokenRequestComponent _authCodeExchangeComponentOverride;

	private readonly ITokenRequestComponent _brokerInteractiveComponent;

	public InteractiveRequest(AuthenticationRequestParameters requestParams, AcquireTokenInteractiveParameters interactiveParameters, IAuthCodeRequestComponent authCodeRequestComponentOverride = null, ITokenRequestComponent authCodeExchangeComponentOverride = null, ITokenRequestComponent brokerExchangeComponentOverride = null)
		: base(requestParams?.RequestContext?.ServiceBundle, requestParams, interactiveParameters)
	{
		_requestParams = requestParams ?? throw new ArgumentNullException("requestParams");
		_interactiveParameters = interactiveParameters ?? throw new ArgumentNullException("interactiveParameters");
		_authCodeRequestComponentOverride = authCodeRequestComponentOverride;
		_authCodeExchangeComponentOverride = authCodeExchangeComponentOverride;
		_brokerInteractiveComponent = brokerExchangeComponentOverride;
		_serviceBundle = requestParams.RequestContext.ServiceBundle;
		_logger = requestParams.RequestContext.Logger;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		cancellationToken.ThrowIfCancellationRequested();
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(await GetTokenResponseAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override void EnrichTelemetryApiEvent(ApiEvent apiEvent)
	{
		apiEvent.Prompt = _interactiveParameters.Prompt.PromptValue;
		if (_interactiveParameters.LoginHint != null)
		{
			apiEvent.LoginHint = _interactiveParameters.LoginHint;
		}
	}

	private async Task<MsalTokenResponse> FetchTokensFromBrokerAsync(string brokerInstallUrl, CancellationToken cancellationToken)
	{
		IBroker broker = _serviceBundle.PlatformProxy.CreateBroker(_interactiveParameters.UiParent);
		return await (_brokerInteractiveComponent ?? new BrokerInteractiveRequestComponent(_requestParams, _interactiveParameters, broker, brokerInstallUrl)).FetchTokensAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<MsalTokenResponse> GetTokenResponseAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		if (_requestParams.IsBrokerConfigured)
		{
			_logger.Info("Broker is configured. Starting broker flow without knowing the broker installation app link.");
			MsalTokenResponse msalTokenResponse = await FetchTokensFromBrokerAsync(null, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (msalTokenResponse != null)
			{
				_logger.Info("Broker attempt completed successfully");
				return msalTokenResponse;
			}
			_logger.Info("Broker attempt did not complete, most likely because the broker is not installed. Attempting to use a browser / web ui");
			cancellationToken.ThrowIfCancellationRequested();
		}
		Tuple<string, string> obj = await (_authCodeRequestComponentOverride ?? new AuthCodeRequestComponent(_requestParams, _interactiveParameters)).FetchAuthCodeAndPkceVerifierAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_logger.Info("An authorization code was retrieved from the /authorize endpoint.");
		string item = obj.Item1;
		string item2 = obj.Item2;
		if (BrokerInteractiveRequestComponent.IsBrokerRequiredAuthCode(item, out var installationUri))
		{
			return await RunBrokerWithInstallUriAsync(installationUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		_logger.Info("Exchanging the auth code for tokens");
		return await (_authCodeExchangeComponentOverride ?? new AuthCodeExchangeComponent(_requestParams, _interactiveParameters, item, item2)).FetchTokensAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<MsalTokenResponse> RunBrokerWithInstallUriAsync(string brokerInstallUri, CancellationToken cancellationToken)
	{
		_logger.Info("Based on the auth code, the broker flow is required. Starting broker flow knowing the broker installation app link.");
		cancellationToken.ThrowIfCancellationRequested();
		MsalTokenResponse msalTokenResponse = await FetchTokensFromBrokerAsync(brokerInstallUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_logger.Info("Broker attempt completed successfully " + (msalTokenResponse != null));
		return msalTokenResponse;
	}
}
