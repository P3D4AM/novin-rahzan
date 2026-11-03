using System;
using Microsoft.Win32;
using PacketParser.Entities;

namespace FormCustom.Assistance;

public class DetectNetFramwork
{
	public static bool GetVersionFromRegistry()
	{
		try
		{
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\");
			if (registryKey != null && registryKey.GetValue("Release") != null)
			{
				return CheckFor45PlusVersion((int)registryKey.GetValue("Release")).CompareTo("4.8.0") >= 0;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVersionFromRegistry", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\DetectNetFramwork.cs", 22);
			return false;
		}
	}

	private static string CheckFor45PlusVersion(int releaseKey)
	{
		try
		{
			if (releaseKey >= 528040)
			{
				return "4.8.0";
			}
			if (releaseKey >= 461808)
			{
				return "4.7.2";
			}
			if (releaseKey >= 461308)
			{
				return "4.7.1";
			}
			if (releaseKey >= 460798)
			{
				return "4.7";
			}
			if (releaseKey >= 394802)
			{
				return "4.6.2";
			}
			if (releaseKey >= 394254)
			{
				return "4.6.1";
			}
			if (releaseKey >= 393295)
			{
				return "4.6";
			}
			if (releaseKey >= 379893)
			{
				return "4.5.2";
			}
			if (releaseKey >= 378675)
			{
				return "4.5.1";
			}
			if (releaseKey >= 378389)
			{
				return "4.5";
			}
			return "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckFor45PlusVersion", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\DetectNetFramwork.cs", 44);
			return "";
		}
	}
}
