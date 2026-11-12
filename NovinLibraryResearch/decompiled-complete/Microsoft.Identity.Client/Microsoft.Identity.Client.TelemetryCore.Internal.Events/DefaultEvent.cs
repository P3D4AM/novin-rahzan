using System.Collections.Generic;
using System.Globalization;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

internal class DefaultEvent : EventBase
{
	public DefaultEvent(IPlatformProxy platformProxy, string correlationId, string clientId, IDictionary<string, int> eventCount)
		: base("msal.default_event", correlationId)
	{
		base["msal.client_id"] = clientId;
		base["msal.sdk_platform"] = platformProxy.GetProductName()?.ToLowerInvariant();
		base["msal.sdk_version"] = MsalIdHelper.GetMsalVersion();
		base["msal.application_name"] = HashPersonalIdentifier(platformProxy.CryptographyManager, platformProxy.GetCallingApplicationName()?.ToLowerInvariant());
		base["msal.application_version"] = HashPersonalIdentifier(platformProxy.CryptographyManager, platformProxy.GetCallingApplicationVersion()?.ToLowerInvariant());
		base["msal.device_id"] = HashPersonalIdentifier(platformProxy.CryptographyManager, platformProxy.GetDeviceId()?.ToLowerInvariant());
		base["Microsoft_MSAL_ui_event_count"] = GetEventCount("msal.ui_event", eventCount);
		base["Microsoft_MSAL_http_event_count"] = GetEventCount("msal.http_event", eventCount);
		base["Microsoft_MSAL_cache_event_count"] = GetEventCount("msal.cache_event", eventCount);
	}

	private string GetEventCount(string eventName, IDictionary<string, int> eventCount)
	{
		if (!eventCount.ContainsKey(eventName))
		{
			return "0";
		}
		return eventCount[eventName].ToString(CultureInfo.InvariantCulture);
	}
}
