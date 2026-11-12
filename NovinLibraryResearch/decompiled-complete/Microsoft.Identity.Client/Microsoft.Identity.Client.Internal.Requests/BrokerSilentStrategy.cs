using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Broker;
using Microsoft.Identity.Client.Internal.Requests.Silent;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class BrokerSilentStrategy : ISilentAuthRequestStrategy
{
	internal AuthenticationRequestParameters _authenticationRequestParameters;

	protected IServiceBundle _serviceBundle;

	private readonly AcquireTokenSilentParameters _silentParameters;

	private readonly SilentRequest _silentRequest;

	private readonly ICoreLogger _logger;

	internal IBroker Broker { get; }

	public BrokerSilentStrategy(SilentRequest request, IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters silentParameters, IBroker broker)
	{
		_authenticationRequestParameters = authenticationRequestParameters;
		_silentParameters = silentParameters;
		_serviceBundle = serviceBundle;
		_silentRequest = request;
		Broker = broker ?? throw new ArgumentNullException("broker");
		_logger = authenticationRequestParameters.RequestContext.Logger;
	}

	public async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		MsalTokenResponse response = await SendTokenRequestToBrokerAsync().ConfigureAwait(continueOnCapturedContext: false);
		return await _silentRequest.CacheTokenResponseAndCreateAuthenticationResultAsync(response).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<MsalTokenResponse> SendTokenRequestToBrokerAsync()
	{
		_authenticationRequestParameters.RequestContext.Logger.Info("Can invoke broker. Will attempt to acquire token with broker. ");
		MsalTokenResponse msalTokenResponse = ((_authenticationRequestParameters.Account == null) ? (await Broker.AcquireTokenSilentDefaultUserAsync(_authenticationRequestParameters, _silentParameters).ConfigureAwait(continueOnCapturedContext: false)) : (await Broker.AcquireTokenSilentAsync(_authenticationRequestParameters, _silentParameters).ConfigureAwait(continueOnCapturedContext: false)));
		ValidateResponseFromBroker(msalTokenResponse);
		return msalTokenResponse;
	}

	internal void ValidateResponseFromBroker(MsalTokenResponse msalTokenResponse)
	{
		_logger.Info("Checking MsalTokenResponse returned from broker. ");
		if (msalTokenResponse.AccessToken != null)
		{
			_logger.Info("Success. Response contains an access token");
			return;
		}
		if (msalTokenResponse.Error != null)
		{
			_logger.Info(LogMessages.ErrorReturnedInBrokerResponse(msalTokenResponse.Error));
			if (msalTokenResponse.Error == "no_tokens_found")
			{
				throw new MsalUiRequiredException(msalTokenResponse.Error, msalTokenResponse.ErrorDescription);
			}
			throw MsalServiceExceptionFactory.FromBrokerResponse(msalTokenResponse.Error, "Broker response returned error: " + msalTokenResponse.ErrorDescription, string.IsNullOrEmpty(msalTokenResponse.SubError) ? "unknown_broker_error" : msalTokenResponse.SubError, msalTokenResponse.CorrelationId, msalTokenResponse.HttpResponse);
		}
		_logger.Info("Unknown error returned in broker response. ");
		throw new MsalServiceException("broker_response_returned_error", "Broker response returned an error which does not contain an error or error description. See https://aka.ms/msal-brokers for details. ", null);
	}
}
