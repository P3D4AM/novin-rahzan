using System;
using System.IO;
using System.Runtime.InteropServices;
using Azure.Core;

namespace Azure.Identity;

internal sealed class MacosVisualStudioCodeAdapter : IVisualStudioCodeAdapter
{
	private static readonly string s_userSettingsJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Application Support", "Code", "User", "settings.json");

	public string GetUserSettingsPath()
	{
		return s_userSettingsJsonPath;
	}

	public string GetCredentials(string serviceName, string accountName)
	{
		Azure.Core.Argument.AssertNotNullOrEmpty(serviceName, "serviceName");
		Azure.Core.Argument.AssertNotNullOrEmpty(accountName, "accountName");
		IntPtr credentialsPtr = IntPtr.Zero;
		IntPtr itemRef = IntPtr.Zero;
		try
		{
			MacosNativeMethods.SecKeychainFindGenericPassword(IntPtr.Zero, serviceName, accountName, out var passwordLength, out credentialsPtr, out itemRef);
			if (passwordLength <= 0)
			{
				throw new InvalidOperationException("No password found");
			}
			return Marshal.PtrToStringAnsi(credentialsPtr, passwordLength);
		}
		finally
		{
			try
			{
				MacosNativeMethods.SecKeychainItemFreeContent(IntPtr.Zero, credentialsPtr);
			}
			finally
			{
				MacosNativeMethods.CFRelease(itemRef);
			}
		}
	}
}
