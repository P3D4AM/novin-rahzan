using System;
using System.IO;

namespace Microsoft.Identity.Client.Extensions.Msal;

public static class SharedUtilities
{
	private static readonly string s_homeEnvVar = Environment.GetEnvironmentVariable("HOME");

	private static readonly string s_lognameEnvVar = Environment.GetEnvironmentVariable("LOGNAME");

	private static readonly string s_userEnvVar = Environment.GetEnvironmentVariable("USER");

	private static readonly string s_lNameEnvVar = Environment.GetEnvironmentVariable("LNAME");

	private static readonly string s_usernameEnvVar = Environment.GetEnvironmentVariable("USERNAME");

	private static readonly Lazy<bool> s_isMono = new Lazy<bool>(() => Type.GetType("Mono.Runtime") != null);

	public static bool IsWindowsPlatform()
	{
		return Environment.OSVersion.Platform == PlatformID.Win32NT;
	}

	public static bool IsMacPlatform()
	{
		if (Environment.OSVersion.Platform != PlatformID.MacOSX)
		{
			return Environment.OSVersion.Platform == PlatformID.Unix;
		}
		return true;
	}

	public static bool IsLinuxPlatform()
	{
		return Environment.OSVersion.Platform == PlatformID.Unix;
	}

	internal static bool IsMonoPlatform()
	{
		return s_isMono.Value;
	}

	public static string GetUserRootDirectory()
	{
		if (IsWindowsPlatform())
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		}
		return GetUserHomeDirOnUnix();
	}

	private static string GetUserHomeDirOnUnix()
	{
		if (IsWindowsPlatform())
		{
			throw new NotSupportedException();
		}
		if (!string.IsNullOrEmpty(s_homeEnvVar))
		{
			return s_homeEnvVar;
		}
		string text = null;
		if (!string.IsNullOrEmpty(s_lognameEnvVar))
		{
			text = s_lognameEnvVar;
		}
		else if (!string.IsNullOrEmpty(s_userEnvVar))
		{
			text = s_userEnvVar;
		}
		else if (!string.IsNullOrEmpty(s_lNameEnvVar))
		{
			text = s_lNameEnvVar;
		}
		else if (!string.IsNullOrEmpty(s_usernameEnvVar))
		{
			text = s_usernameEnvVar;
		}
		if (IsMacPlatform())
		{
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return Path.Combine("/Users", text);
		}
		if (IsLinuxPlatform())
		{
			if (LinuxNativeMethods.getuid() == 0)
			{
				return "/root";
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return Path.Combine("/home", text);
		}
		throw new NotSupportedException();
	}
}
