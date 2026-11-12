using System;
using System.ComponentModel;

namespace Microsoft.Identity.Client;

[Obsolete("In MSAL.NET 3.x, you should directly pass the Activity (on Xamarin.Android), or Window (on .NET Framework and UWP) using AcquireTokenInteractiveParameterBuilder.WithParentActivityOrWindowSee https://aka.ms/msal-net-3-breaking-changes", true)]
public sealed class UIParent
{
	[Obsolete("See https://aka.ms/msal-net-3-breaking-changes", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public UIParent()
	{
		throw new NotImplementedException("See https://aka.ms/msal-net-3-breaking-changes");
	}

	[Obsolete("See https://aka.ms/msal-net-3-breaking-changes", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public UIParent(object parent, bool useEmbeddedWebView)
	{
		throw new NotImplementedException("See https://aka.ms/msal-net-3-breaking-changes");
	}

	[Obsolete("See https://aka.ms/msal-net-3-breaking-changes", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsSystemWebviewAvailable()
	{
		throw new NotImplementedException("See https://aka.ms/msal-net-3-breaking-changes");
	}
}
