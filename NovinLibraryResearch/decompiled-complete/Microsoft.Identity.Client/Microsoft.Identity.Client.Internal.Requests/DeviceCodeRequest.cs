using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class DeviceCodeRequest : RequestBase
{
	private readonly AcquireTokenWithDeviceCodeParameters _deviceCodeParameters;

	public DeviceCodeRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenWithDeviceCodeParameters deviceCodeParameters)
		: base(serviceBundle, authenticationRequestParameters, deviceCodeParameters)
	{
		_deviceCodeParameters = deviceCodeParameters;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		OAuth2Client oAuth2Client = new OAuth2Client(base.ServiceBundle.DefaultLogger, base.ServiceBundle.HttpManager, base.ServiceBundle.MatsTelemetryManager);
		HashSet<string> deviceCodeScopes = new HashSet<string>();
		deviceCodeScopes.UnionWith(base.AuthenticationRequestParameters.Scope);
		deviceCodeScopes.Add("offline_access");
		deviceCodeScopes.Add("profile");
		deviceCodeScopes.Add("openid");
		oAuth2Client.AddBodyParameter("client_id", base.AuthenticationRequestParameters.ClientId);
		oAuth2Client.AddBodyParameter("scope", deviceCodeScopes.AsSingleString());
		oAuth2Client.AddBodyParameter("claims", base.AuthenticationRequestParameters.ClaimsAndClientCapabilities);
		UriBuilder uriBuilder = new UriBuilder(base.AuthenticationRequestParameters.Endpoints.DeviceCodeEndpoint);
		uriBuilder.AppendQueryParameters(base.AuthenticationRequestParameters.ExtraQueryParameters);
		DeviceCodeResult deviceCodeResult = (await oAuth2Client.ExecuteRequestAsync<DeviceCodeResponse>(uriBuilder.Uri, HttpMethod.Post, base.AuthenticationRequestParameters.RequestContext, expectErrorsOn200OK: true).ConfigureAwait(continueOnCapturedContext: false)).GetResult(base.AuthenticationRequestParameters.ClientId, deviceCodeScopes);
		await _deviceCodeParameters.DeviceCodeResultCallback(deviceCodeResult).ConfigureAwait(continueOnCapturedContext: false);
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(await WaitForTokenResponseAsync(deviceCodeResult, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<MsalTokenResponse> WaitForTokenResponseAsync(DeviceCodeResult deviceCodeResult, CancellationToken cancellationToken)
	{
		TimeSpan timeRemaining = deviceCodeResult.ExpiresOn - DateTimeOffset.UtcNow;
		while (timeRemaining.TotalSeconds > 0.0)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException();
			}
			try
			{
				return await SendTokenRequestAsync(base.AuthenticationRequestParameters.Endpoints.TokenEndpoint, GetBodyParameters(deviceCodeResult), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (MsalServiceException ex)
			{
				if (!ex.ErrorCode.Equals("authorization_pending", StringComparison.OrdinalIgnoreCase))
				{
					throw;
				}
				await Task.Delay(TimeSpan.FromSeconds(deviceCodeResult.Interval), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				timeRemaining = deviceCodeResult.ExpiresOn - DateTimeOffset.UtcNow;
			}
		}
		throw new MsalClientException("code_expired", "Verification code expired before contacting the server");
	}

	private Dictionary<string, string> GetBodyParameters(DeviceCodeResult deviceCodeResult)
	{
		return new Dictionary<string, string>
		{
			["grant_type"] = "device_code",
			["device_code"] = deviceCodeResult.DeviceCode
		};
	}
}
