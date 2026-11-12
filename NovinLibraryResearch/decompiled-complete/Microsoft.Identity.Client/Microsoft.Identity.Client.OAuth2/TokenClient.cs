using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.OAuth2;

internal class TokenClient
{
	private readonly AuthenticationRequestParameters _requestParams;

	private readonly IServiceBundle _serviceBundle;

	private readonly OAuth2Client _oAuth2Client;

	private volatile bool _requestInProgress;

	public TokenClient(AuthenticationRequestParameters requestParams)
	{
		_requestParams = requestParams ?? throw new ArgumentNullException("requestParams");
		_serviceBundle = _requestParams.RequestContext.ServiceBundle;
		_oAuth2Client = new OAuth2Client(_serviceBundle.DefaultLogger, _serviceBundle.HttpManager, _serviceBundle.MatsTelemetryManager);
	}

	public async Task<MsalTokenResponse> SendTokenRequestAsync(IDictionary<string, string> additionalBodyParameters, string scopeOverride = null, string tokenEndpointOverride = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		cancellationToken.ThrowIfCancellationRequested();
		string tokenEndpoint = tokenEndpointOverride ?? _requestParams.Endpoints.TokenEndpoint;
		string scopes = ((!string.IsNullOrEmpty(scopeOverride)) ? scopeOverride : GetDefaultScopes(_requestParams.Scope));
		AddBodyParamsAndHeaders(additionalBodyParameters, scopes);
		AddThrottlingHeader();
		_serviceBundle.ThrottlingManager.TryThrottle(_requestParams, _oAuth2Client.GetBodyParameters());
		MsalTokenResponse msalTokenResponse;
		try
		{
			msalTokenResponse = await SendHttpAndClearTelemetryAsync(tokenEndpoint).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (MsalServiceException ex)
		{
			_serviceBundle.ThrottlingManager.RecordException(_requestParams, _oAuth2Client.GetBodyParameters(), ex);
			throw;
		}
		if (string.IsNullOrEmpty(msalTokenResponse.Scope))
		{
			msalTokenResponse.Scope = _requestParams.Scope.AsSingleString();
			_requestParams.RequestContext.Logger.Info("ScopeSet was missing from the token response, so using developer provided scopes in the result. ");
		}
		if (!string.IsNullOrEmpty(msalTokenResponse.TokenType) && !string.Equals(msalTokenResponse.TokenType, _requestParams.AuthenticationScheme.AccessTokenType, StringComparison.OrdinalIgnoreCase))
		{
			throw new MsalClientException("token_type_mismatch", MsalErrorMessage.TokenTypeMismatch(_requestParams.AuthenticationScheme.AccessTokenType, msalTokenResponse.TokenType));
		}
		return msalTokenResponse;
	}

	private void AddThrottlingHeader()
	{
		_oAuth2Client.AddHeader("x-ms-lib-capability", "retry-after, h429");
	}

	private void AddBodyParamsAndHeaders(IDictionary<string, string> additionalBodyParameters, string scopes)
	{
		_oAuth2Client.AddBodyParameter("client_id", _requestParams.ClientId);
		_oAuth2Client.AddBodyParameter("client_info", "1");
		if (_requestParams.ClientCredential != null)
		{
			foreach (KeyValuePair<string, string> item in ClientCredentialHelper.CreateClientCredentialBodyParameters(_requestParams.RequestContext.Logger, _serviceBundle.PlatformProxy.CryptographyManager, _requestParams.ClientCredential, _requestParams.ClientId, _requestParams.Endpoints, _requestParams.SendX5C))
			{
				_oAuth2Client.AddBodyParameter(item.Key, item.Value);
			}
		}
		_oAuth2Client.AddBodyParameter("scope", scopes);
		_oAuth2Client.AddBodyParameter("claims", _requestParams.ClaimsAndClientCapabilities);
		foreach (KeyValuePair<string, string> additionalBodyParameter in additionalBodyParameters)
		{
			_oAuth2Client.AddBodyParameter(additionalBodyParameter.Key, additionalBodyParameter.Value);
		}
		foreach (KeyValuePair<string, string> tokenRequestParam in _requestParams.AuthenticationScheme.GetTokenRequestParams())
		{
			_oAuth2Client.AddBodyParameter(tokenRequestParam.Key, tokenRequestParam.Value);
		}
		_oAuth2Client.AddHeader("x-client-current-telemetry", _serviceBundle.HttpTelemetryManager.GetCurrentRequestHeader(_requestParams.RequestContext.ApiEvent));
		if (!_requestInProgress)
		{
			_requestInProgress = true;
			_oAuth2Client.AddHeader("x-client-last-telemetry", _serviceBundle.HttpTelemetryManager.GetLastRequestHeader());
		}
		_oAuth2Client.AddHeader("x-ms-PKeyAuth", "1.0");
	}

	private async Task<MsalTokenResponse> SendHttpAndClearTelemetryAsync(string tokenEndpoint)
	{
		UriBuilder builder = new UriBuilder(tokenEndpoint);
		try
		{
			builder.AppendQueryParameters(_requestParams.ExtraQueryParameters);
			MsalTokenResponse result = await _oAuth2Client.GetTokenAsync(builder.Uri, _requestParams.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
			_serviceBundle.HttpTelemetryManager.ResetPreviousUnsentData();
			return result;
		}
		catch (MsalServiceException ex)
		{
			if (!ex.IsAadUnavailable())
			{
				_serviceBundle.HttpTelemetryManager.ResetPreviousUnsentData();
			}
			if (ex.StatusCode == 401)
			{
				string responseHeader = string.Empty;
				if (_serviceBundle.DeviceAuthManager.TryCreateDeviceAuthChallengeResponseAsync(ex.Headers, builder.Uri, out responseHeader))
				{
					_oAuth2Client.AddHeader("Authorization", responseHeader);
					return await _oAuth2Client.GetTokenAsync(builder.Uri, _requestParams.RequestContext, addCommonHeaders: false).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			throw;
		}
		finally
		{
			_requestInProgress = false;
		}
	}

	private static string GetDefaultScopes(ISet<string> inputScope)
	{
		SortedSet<string> sortedSet = new SortedSet<string>(inputScope.ToArray(), StringComparer.OrdinalIgnoreCase);
		sortedSet.UnionWith(OAuth2Value.ReservedScopes);
		return sortedSet.AsSingleString();
	}
}
