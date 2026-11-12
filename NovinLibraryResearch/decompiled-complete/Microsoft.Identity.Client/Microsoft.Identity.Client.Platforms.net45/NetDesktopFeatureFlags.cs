using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class NetDesktopFeatureFlags : IFeatureFlags
{
	public bool IsFociEnabled => true;
}
