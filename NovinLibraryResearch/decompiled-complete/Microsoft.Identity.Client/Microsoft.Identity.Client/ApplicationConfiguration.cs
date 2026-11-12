using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client;

internal sealed class ApplicationConfiguration : IApplicationConfiguration, IAppConfig
{
	public const string DefaultClientName = "UnknownClient";

	public const string DefaultClientVersion = "0.0.0.0";

	private string _clientName = "UnknownClient";

	private string _clientVersion = "0.0.0.0";

	internal int ConfidentialClientCredentialCount;

	public string ClientName
	{
		get
		{
			return _clientName;
		}
		internal set
		{
			_clientName = (string.IsNullOrWhiteSpace(value) ? "UnknownClient" : value);
		}
	}

	public string ClientVersion
	{
		get
		{
			return _clientVersion;
		}
		internal set
		{
			_clientVersion = (string.IsNullOrWhiteSpace(value) ? "0.0.0.0" : value);
		}
	}

	public Func<object> ParentActivityOrWindowFunc { get; internal set; }

	public bool UseCorporateNetwork { get; internal set; }

	public string IosKeychainSecurityGroup { get; internal set; }

	public bool IsBrokerEnabled { get; internal set; }

	public ITelemetryConfig TelemetryConfig { get; internal set; }

	public IHttpManager HttpManager { get; internal set; }

	public IPlatformProxy PlatformProxy { get; internal set; }

	public AuthorityInfo AuthorityInfo { get; internal set; }

	public string ClientId { get; internal set; }

	public string TenantId { get; internal set; }

	public string RedirectUri { get; internal set; }

	public bool EnablePiiLogging { get; internal set; }

	public LogLevel LogLevel { get; internal set; } = LogLevel.Info;


	public bool IsDefaultPlatformLoggingEnabled { get; internal set; }

	public IMsalHttpClientFactory HttpClientFactory { get; internal set; }

	public bool IsExtendedTokenLifetimeEnabled { get; set; }

	public TelemetryCallback TelemetryCallback { get; internal set; }

	public LogCallback LoggingCallback { get; internal set; }

	public string Component { get; internal set; }

	public IDictionary<string, string> ExtraQueryParameters { get; internal set; } = new Dictionary<string, string>();


	public bool UseRecommendedDefaultRedirectUri { get; internal set; }

	internal ILegacyCachePersistence UserTokenLegacyCachePersistenceForTest { get; set; }

	internal ILegacyCachePersistence AppTokenLegacyCachePersistenceForTest { get; set; }

	public bool ExperimentalFeaturesEnabled { get; set; }

	public IEnumerable<string> ClientCapabilities { get; set; }

	public ClientCredentialWrapper ClientCredential { get; internal set; }

	public string ClientSecret { get; internal set; }

	public string SignedClientAssertion { get; internal set; }

	public X509Certificate2 ClientCredentialCertificate { get; internal set; }

	public IDictionary<string, string> ClaimsToSign { get; internal set; }

	public bool MergeWithDefaultClaims { get; internal set; }

	public InstanceDiscoveryResponse CustomInstanceDiscoveryMetadata { get; set; }

	public Uri CustomInstanceDiscoveryMetadataUri { get; set; }

	internal AadAuthorityAudience AadAuthorityAudience { get; set; }

	internal AzureCloudInstance AzureCloudInstance { get; set; }

	internal string Instance { get; set; }

	internal bool ValidateAuthority { get; set; }
}
