using System;
using Microsoft.Win32;
using PacketParser.Entities;

namespace FormCustom.ManagementPassword;

public class CLSRegistry
{
	public static string Read(string key)
	{
		try
		{
			return Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin", key, "")?.ToString() ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Read", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\CLSRegistry.cs", 18);
		}
		return "";
	}

	public static void Write(string key, string value)
	{
		try
		{
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin", key, value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Write", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\CLSRegistry.cs", 29);
		}
	}

	public static string Read(string key, string dll)
	{
		try
		{
			return Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin\\" + dll.ToUpperInvariant(), key, "").ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Read", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\CLSRegistry.cs", 40);
			return "";
		}
	}

	public static void Write(string key, string value, string dll)
	{
		try
		{
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\" + dll.ToUpperInvariant(), key, value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Write", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\CLSRegistry.cs", 51);
		}
	}
}
