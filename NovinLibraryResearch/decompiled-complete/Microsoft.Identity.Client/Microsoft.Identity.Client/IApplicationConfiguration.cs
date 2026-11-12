using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client;

internal interface IApplicationConfiguration : IAppConfig
{
	bool IsExtendedTokenLifetimeEnabled { get; }

	AuthorityInfo AuthorityInfo { get; }

	InstanceDiscoveryResponse CustomInstanceDiscoveryMetadata { get; }

	ClientCredentialWrapper ClientCredential { get; }

	TelemetryCallback TelemetryCallback { get; }
}
