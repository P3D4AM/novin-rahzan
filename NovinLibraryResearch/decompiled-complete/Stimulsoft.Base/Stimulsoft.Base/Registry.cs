using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Stimulsoft.Base;

public class Registry
{
	public enum RegSAM
	{
		QueryValue = 1,
		SetValue = 2,
		CreateSubKey = 4,
		EnumerateSubKeys = 8,
		Notify = 16,
		CreateLink = 32,
		WOW64_32Key = 512,
		WOW64_64Key = 256,
		WOW64_Res = 768,
		Read = 131097,
		Write = 131078,
		Execute = 131097,
		AllAccess = 983103
	}

	public static class RegHive
	{
		internal static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(2147483650u);

		internal static UIntPtr HKEY_CURRENT_USER = new UIntPtr(2147483649u);
	}

	public static class RegistryWOW6432
	{
		[DllImport("Advapi32.dll")]
		private static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions, int samDesired, out UIntPtr phkResult);

		[DllImport("Advapi32.dll")]
		private static extern uint RegCloseKey(UIntPtr hKey);

		[DllImport("advapi32.dll")]
		internal static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName, int lpReserved, ref uint lpType, StringBuilder lpData, ref uint lpcbData);

		internal static string GetRegKey64(UIntPtr inHive, string inKeyName, string inPropertyName)
		{
			return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName);
		}

		internal static string GetRegKey32(UIntPtr inHive, string inKeyName, string inPropertyName)
		{
			return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName);
		}

		internal static string GetRegKey64(UIntPtr inHive, string inKeyName, RegSAM in32or64key, string inPropertyName)
		{
			UIntPtr phkResult = UIntPtr.Zero;
			try
			{
				if (RegOpenKeyEx(RegHive.HKEY_LOCAL_MACHINE, inKeyName, 0u, (int)(RegSAM.QueryValue | in32or64key), out phkResult) != 0)
				{
					return null;
				}
				uint lpType = 0u;
				uint lpcbData = 1024u;
				StringBuilder stringBuilder = new StringBuilder(1024);
				RegQueryValueEx(phkResult, inPropertyName, 0, ref lpType, stringBuilder, ref lpcbData);
				return stringBuilder.ToString();
			}
			finally
			{
				if (UIntPtr.Zero != phkResult)
				{
					RegCloseKey(phkResult);
				}
			}
		}
	}

	public static string GetValue(string name)
	{
		try
		{
			string text = null;
			try
			{
				RegistryKey val = null;
				if (Environment.Is64BitOperatingSystem)
				{
					RegistryKey val2 = RegistryKey.OpenBaseKey((RegistryHive)(-2147483646), (RegistryView)256);
					val = val2.OpenSubKey("SOFTWARE\\Stimulsoft\\Stimulsoft Reports");
				}
				if (val == null)
				{
					RegistryKey val2 = RegistryKey.OpenBaseKey((RegistryHive)(-2147483646), (RegistryView)512);
					val = val2.OpenSubKey("SOFTWARE\\Stimulsoft\\Stimulsoft Reports");
				}
				if (val != null)
				{
					text = val.GetValue(name) as string;
				}
				if (text != null)
				{
					return text;
				}
			}
			catch
			{
			}
			try
			{
				RegistryKey val3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Stimulsoft\\Stimulsoft Reports");
				if (val3 != null)
				{
					text = val3.GetValue(name) as string;
				}
				if (text != null)
				{
					return text;
				}
			}
			catch
			{
			}
			string regKey = RegistryWOW6432.GetRegKey32(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Stimulsoft\\\\Stimulsoft Reports", name);
			if (regKey != null)
			{
				return regKey;
			}
			return RegistryWOW6432.GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Stimulsoft\\\\Stimulsoft Reports", name);
		}
		catch
		{
		}
		return null;
	}
}
