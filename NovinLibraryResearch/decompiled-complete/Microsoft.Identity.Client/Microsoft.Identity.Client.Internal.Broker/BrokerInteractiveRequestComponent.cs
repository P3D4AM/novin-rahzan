using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal.Broker;

internal class BrokerInteractiveRequestComponent : ITokenRequestComponent
{
	private readonly AcquireTokenInteractiveParameters _interactiveParameters;

	private readonly string _optionalBrokerInstallUrl;

	private readonly AuthenticationRequestParameters _authenticationRequestParameters;

	private readonly IServiceBundle _serviceBundle;

	private readonly ICoreLogger _logger;

	internal IBroker Broker { get; }

	public BrokerInteractiveRequestComponent(AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenInteractiveParameters acquireTokenInteractiveParameters, IBroker broker, string optionalBrokerInstallUrl)
	{
		_authenticationRequestParameters = authenticationRequestParameters;
		_interactiveParameters = acquireTokenInteractiveParameters;
		_serviceBundle = authenticationRequestParameters.RequestContext.ServiceBundle;
		Broker = broker;
		_optionalBrokerInstallUrl = optionalBrokerInstallUrl;
		_logger = _authenticationRequestParameters.RequestContext.Logger;
	}

	public async Task<MsalTokenResponse> FetchTokensAsync(CancellationToken cancellationToken)
	{
		if (Broker.IsBrokerInstalledAndInvokable())
		{
			_logger.Info("Can invoke broker. Will attempt to acquire token with broker. ");
		}
		else
		{
			if (string.IsNullOrEmpty(_optionalBrokerInstallUrl))
			{
				_logger.Info("Broker is required but not installed. An app uri has not been provided.");
				return null;
			}
			_logger.Info("Broker is required for authentication and broker is not installed on the device. Adding BrokerInstallUrl to broker payload. ");
			Broker.HandleInstallUrl(_optionalBrokerInstallUrl);
		}
		MsalTokenResponse msalTokenResponse = await Broker.AcquireTokenInteractiveAsync(_authenticationRequestParameters, _interactiveParameters).ConfigureAwait(continueOnCapturedContext: false);
		ValidateResponseFromBroker(msalTokenResponse);
		return msalTokenResponse;
	}

	internal void ValidateResponseFromBroker(MsalTokenResponse msalTokenResponse)
	{
		_logger.Info("Checking MsalTokenResponse returned from broker. ");
		if (!string.IsNullOrEmpty(msalTokenResponse.AccessToken))
		{
			_logger.Info("Success. Broker response contains an access token");
			return;
		}
		if (msalTokenResponse.Error != null)
		{
			_logger.Info(LogMessages.ErrorReturnedInBrokerResponse(msalTokenResponse.Error));
			throw MsalServiceExceptionFactory.FromBrokerResponse(msalTokenResponse.Error, "Broker response returned error: " + msalTokenResponse.ErrorDescription, string.IsNullOrEmpty(msalTokenResponse.SubError) ? "unknown_broker_error" : msalTokenResponse.SubError, msalTokenResponse.CorrelationId, msalTokenResponse.HttpResponse);
		}
		_logger.Info("Unknown error returned in broker response. ");
		throw new MsalServiceException("broker_response_returned_error", "Broker response returned an error which does not contain an error or error description. See https://aka.ms/msal-brokers for details. ", null);
	}

	public static bool IsBrokerRequiredAuthCode(string authCode, out string installationUri)
	{
		if (authCode.StartsWith("msauth://", StringComparison.OrdinalIgnoreCase))
		{
			installationUri = ExtractAppLink(authCode);
			return installationUri != null;
		}
		installationUri = null;
		return false;
	}

	private static string ExtractAppLink(string authCode)
	{
		string text = new Uri(authCode).Query;
		if (text.StartsWith("?", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(1);
		}
		Dictionary<string, string> dictionary = CoreHelpers.ParseKeyValueList(text, '&', urlDecode: true, lowercaseKeys: true, null);
		if (!dictionary.ContainsKey("app_link"))
		{
			return null;
		}
		return dictionary["app_link"];
	}
}
