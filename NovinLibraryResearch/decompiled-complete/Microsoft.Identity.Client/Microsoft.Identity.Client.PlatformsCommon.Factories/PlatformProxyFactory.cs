using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Logger;
using Microsoft.Identity.Client.Platforms.net45;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.PlatformsCommon.Factories;

internal static class PlatformProxyFactory
{
	public static IPlatformProxy CreatePlatformProxy(ICoreLogger logger)
	{
		return new NetDesktopPlatformProxy(logger ?? MsalLogger.NullLogger);
	}
}
