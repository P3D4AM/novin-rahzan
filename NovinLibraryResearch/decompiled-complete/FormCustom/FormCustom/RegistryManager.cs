using System;
using Microsoft.Win32;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom;

public class RegistryManager
{
	public static long GetLkSerial()
	{
		try
		{
			return (Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "LKSerial", "")?.ToString()?.ParseToLong(0L)).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLkSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\RegistryManager.cs", 19);
			return 0L;
		}
	}
}
