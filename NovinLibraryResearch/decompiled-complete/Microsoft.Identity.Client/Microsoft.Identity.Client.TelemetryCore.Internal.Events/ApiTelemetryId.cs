namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

internal enum ApiTelemetryId
{
	Unknown,
	AcquireTokenSilent,
	AcquireTokenWithDeviceCode,
	GetAuthorizationRequestUrl,
	AcquireTokenOnBehalfOf,
	AcquireTokenInteractive,
	AcquireTokenForClient,
	AcquireTokenByUsernamePassword,
	AcquireTokenByRefreshToken,
	AcquireTokenByIntegratedWindowsAuth,
	AcquireTokenByAuthorizationCode
}
