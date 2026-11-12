using System;
using System.ComponentModel;

namespace Microsoft.Identity.Client;

[Obsolete("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
public sealed class Logger
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public static LogCallback LogCallback
	{
		set
		{
			throw new NotImplementedException("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public static LogLevel Level
	{
		get
		{
			throw new NotImplementedException("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
		}
		set
		{
			throw new NotImplementedException("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public static bool PiiLoggingEnabled { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Logging is now specified per ClientApplication.  See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-application-configuration", true)]
	public static bool DefaultLoggingEnabled { get; set; }
}
