using System;
using Microsoft.Win32;
using PacketParser.Entities;

namespace PayamakSettings.FlrSettings;

public class Registry
{
	public static string Read(string key)
	{
		try
		{
			return Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin\\Payamak", key, (object)"")?.ToString() ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Read", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\FlrSettings\\Registry.cs", 20);
			return "";
		}
	}

	public static int ReadInt(string key)
	{
		try
		{
			int.TryParse(Read(key), out var result);
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadInt", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\FlrSettings\\Registry.cs", 35);
			return 0;
		}
	}

	public static bool ReadBool(string key)
	{
		try
		{
			string text = Read(key);
			return text != "" && bool.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadBool", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\FlrSettings\\Registry.cs", 49);
			return false;
		}
	}

	public static bool Write(string key, object value)
	{
		try
		{
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\Payamak", key, value);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Write", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\FlrSettings\\Registry.cs", 63);
			return false;
		}
	}
}
