using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ContextStore
{
	public TelemetryAudienceType AudienceType { get; }

	public string AppName { get; }

	public string AppVersion { get; }

	public string DptiInternal { get; }

	public string DeviceNetworkState { get; }

	public string SessionId { get; }

	public int Platform { get; }

	public static ContextStore CreateContextStore(TelemetryAudienceType audienceType, string appName, string appVersion, string dptiInternal, string deviceNetworkState, string sessionId, int platform)
	{
		if (!Guid.TryParse(sessionId, out var _))
		{
			sessionId = MatsId.Create();
		}
		return new ContextStore(audienceType, appName, appVersion, dptiInternal, deviceNetworkState, sessionId, platform);
	}

	private ContextStore(TelemetryAudienceType audienceType, string appName, string appVersion, string dptiInternal, string deviceNetworkState, string sessionId, int platform)
	{
		AudienceType = audienceType;
		AppName = appName;
		AppVersion = appVersion;
		DptiInternal = dptiInternal;
		DeviceNetworkState = deviceNetworkState;
		SessionId = sessionId;
		Platform = platform;
	}

	public void AddContext(IEnumerable<IPropertyBag> propertyBags)
	{
		foreach (IPropertyBag propertyBag in propertyBags)
		{
			propertyBag.Add("appaudience", MatsConverter.AsString(AudienceType));
			propertyBag.Add("appname", AppName);
			propertyBag.Add("appver", AppVersion);
			propertyBag.Add("devicenetworkstate", DeviceNetworkState);
			propertyBag.Add("deviceprofiletelemetryid", DptiInternal);
			propertyBag.Add("sessionid", SessionId);
			propertyBag.Add("eventtype", "authentication");
			propertyBag.Add("sdkver", "1.1.0");
			propertyBag.Add("platform", Platform);
		}
	}
}
