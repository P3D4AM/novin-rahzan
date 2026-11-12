using System.Globalization;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class UploadEventUtils
{
	public static string GetUploadEventName(IPlatformProxy platformProxy, EventType eventType, string appName)
	{
		string text = MatsConverter.AsString(eventType);
		return GetUploadEventNameGeneric(osPlatform: platformProxy.GetMatsOsPlatform().ToUpperInvariant(), eventName: text.ToUpperInvariant(), appName: appName.ToUpperInvariant());
	}

	private static string GetUploadEventNameGeneric(string eventName, string appName, string osPlatform)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", eventName, appName, osPlatform);
	}
}
