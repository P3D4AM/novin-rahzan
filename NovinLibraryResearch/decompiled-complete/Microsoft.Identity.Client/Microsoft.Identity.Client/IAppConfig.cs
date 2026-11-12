using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Identity.Client;

public interface IAppConfig
{
	string ClientId { get; }

	bool EnablePiiLogging { get; }

	IMsalHttpClientFactory HttpClientFactory { get; }

	LogLevel LogLevel { get; }

	bool IsDefaultPlatformLoggingEnabled { get; }

	string RedirectUri { get; }

	string TenantId { get; }

	LogCallback LoggingCallback { get; }

	IDictionary<string, string> ExtraQueryParameters { get; }

	bool IsBrokerEnabled { get; }

	string ClientName { get; }

	string ClientVersion { get; }

	ITelemetryConfig TelemetryConfig { get; }

	bool ExperimentalFeaturesEnabled { get; }

	IEnumerable<string> ClientCapabilities { get; }

	string ClientSecret { get; }

	X509Certificate2 ClientCredentialCertificate { get; }

	Func<object> ParentActivityOrWindowFunc { get; }
}
