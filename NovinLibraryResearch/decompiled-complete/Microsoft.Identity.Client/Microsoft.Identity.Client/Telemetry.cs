using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Identity.Client.TelemetryCore;

namespace Microsoft.Identity.Client;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
public class Telemetry : ITelemetryReceiver
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public delegate void Receiver(List<Dictionary<string, string>> events);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public bool TelemetryOnFailureOnly
	{
		get
		{
			throw new NotImplementedException("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
		}
		set
		{
			throw new NotImplementedException("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public static Telemetry GetInstance()
	{
		throw new NotImplementedException("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public void RegisterReceiver(Receiver r)
	{
		throw new NotImplementedException("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public bool HasRegisteredReceiver()
	{
		throw new NotImplementedException("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	void ITelemetryReceiver.HandleTelemetryEvents(List<Dictionary<string, string>> events)
	{
		throw new NotImplementedException("Telemetry is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
	}
}
