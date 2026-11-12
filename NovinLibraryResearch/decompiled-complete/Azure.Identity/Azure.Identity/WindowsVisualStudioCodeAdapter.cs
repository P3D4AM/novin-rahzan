using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Azure.Identity;

internal sealed class WindowsVisualStudioCodeAdapter : IVisualStudioCodeAdapter
{
	private static readonly string s_userSettingsJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Code", "User", "settings.json");

	public string GetUserSettingsPath()
	{
		return s_userSettingsJsonPath;
	}

	public string GetCredentials(string serviceName, string accountName)
	{
		IntPtr intPtr = WindowsNativeMethods.CredRead(serviceName + "/" + accountName, WindowsNativeMethods.CRED_TYPE.GENERIC);
		try
		{
			WindowsNativeMethods.CredentialData credentialData = Marshal.PtrToStructure<WindowsNativeMethods.CredentialData>(intPtr);
			return Marshal.PtrToStringAnsi(credentialData.CredentialBlob, (int)credentialData.CredentialBlobSize);
		}
		finally
		{
			WindowsNativeMethods.CredFree(intPtr);
		}
	}
}
