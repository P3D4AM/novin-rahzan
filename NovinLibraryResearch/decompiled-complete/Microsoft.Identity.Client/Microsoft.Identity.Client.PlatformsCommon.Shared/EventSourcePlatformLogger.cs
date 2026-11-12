using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.PlatformsCommon.Shared;

internal class EventSourcePlatformLogger : IPlatformLogger
{
	internal static MsalEventSource MsalEventSource { get; }

	static EventSourcePlatformLogger()
	{
		MsalEventSource = new MsalEventSource();
	}

	public void Error(string message)
	{
		MsalEventSource.Error(message);
	}

	public void Warning(string message)
	{
		MsalEventSource.Error(message);
	}

	public void Verbose(string message)
	{
		MsalEventSource.Error(message);
	}

	public void Information(string message)
	{
		MsalEventSource.Error(message);
	}
}
