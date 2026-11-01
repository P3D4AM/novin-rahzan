using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.Net.WebSockets.WebSocketProtocol;

namespace System;

internal static class SR
{
	private static ResourceManager s_resourceManager;

	private static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(ResourceType));

	internal static Type ResourceType { get; } = typeof(SR);


	internal static string net_WebSockets_InvalidEmptySubProtocol => GetResourceString("net_WebSockets_InvalidEmptySubProtocol", null);

	internal static string net_WebSockets_InvalidState => GetResourceString("net_WebSockets_InvalidState", null);

	internal static string net_WebSockets_InvalidCharInProtocolString => GetResourceString("net_WebSockets_InvalidCharInProtocolString", null);

	internal static string net_WebSockets_ReasonNotNull => GetResourceString("net_WebSockets_ReasonNotNull", null);

	internal static string net_WebSockets_InvalidCloseStatusCode => GetResourceString("net_WebSockets_InvalidCloseStatusCode", null);

	internal static string net_WebSockets_InvalidCloseStatusDescription => GetResourceString("net_WebSockets_InvalidCloseStatusDescription", null);

	internal static string net_WebSockets_UnsupportedPlatform => GetResourceString("net_WebSockets_UnsupportedPlatform", null);

	internal static string net_WebSockets_Argument_InvalidMessageType => GetResourceString("net_WebSockets_Argument_InvalidMessageType", null);

	internal static string net_Websockets_AlreadyOneOutstandingOperation => GetResourceString("net_Websockets_AlreadyOneOutstandingOperation", null);

	internal static string NotReadableStream => GetResourceString("NotReadableStream", null);

	internal static string NotWriteableStream => GetResourceString("NotWriteableStream", null);

	internal static string net_WebSockets_ArgumentOutOfRange_TooSmall => GetResourceString("net_WebSockets_ArgumentOutOfRange_TooSmall", null);

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool UsingResourceKeys()
	{
		return false;
	}

	internal static string GetResourceString(string resourceKey, string defaultString)
	{
		string text = null;
		try
		{
			text = ResourceManager.GetString(resourceKey);
		}
		catch (MissingManifestResourceException)
		{
		}
		if (defaultString != null && resourceKey.Equals(text, StringComparison.Ordinal))
		{
			return defaultString;
		}
		return text;
	}

	internal static string Format(string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			if (UsingResourceKeys())
			{
				return resourceFormat + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}
		return resourceFormat;
	}

	internal static string Format(string resourceFormat, object p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(resourceFormat, p1);
	}

	internal static string Format(string resourceFormat, object p1, object p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(resourceFormat, p1, p2);
	}

	internal static string Format(string resourceFormat, object p1, object p2, object p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(resourceFormat, p1, p2, p3);
	}
}
