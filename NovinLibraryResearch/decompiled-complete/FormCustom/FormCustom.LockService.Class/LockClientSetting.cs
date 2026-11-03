using System;
using System.Threading.Tasks;
using EntityCache.Business.LkService.Interfaces;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.LockService.Class;

public class LockClientSetting : ILkClient
{
	public const string KeyName = "LockSettings";

	public int Port { get; set; }

	public string Ip { get; set; }

	public string SetUpIp { get; set; }

	public long LkSerial { get; set; }

	public bool IsClient { get; set; }

	public LockClientSetting()
	{
	}

	public LockClientSetting(string connectionIp, string setUpIp, int port)
	{
		try
		{
			Ip = connectionIp;
			SetUpIp = setUpIp;
			Port = port;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockClientSetting.cs", 29);
		}
	}

	public static Task SetSettingAsync(LockClientSetting lockInfo)
	{
		if (lockInfo == null)
		{
			WebErrorLog.Instance.StartLog("lockInfo cannot be null.", "", "SetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockClientSetting.cs", 36);
			return Task.CompletedTask;
		}
		try
		{
			using (new JaegerMonitor("SetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockClientSetting.cs"))
			{
				EnvironmentManager<LockClientSetting>.SetValue("LockSettings", lockInfo);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Error setting lockInfo: {lockInfo}", "SetSettingAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockClientSetting.cs", 44);
		}
		return Task.CompletedTask;
	}

	public static LockClientSetting GetSetting()
	{
		try
		{
			LockClientSetting lockClientSetting = EnvironmentManager<LockClientSetting>.GetValue("LockSettings");
			if (lockClientSetting == null)
			{
				lockClientSetting = new LockClientSetting();
			}
			return lockClientSetting;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSetting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockClientSetting.cs", 57);
			return null;
		}
	}
}
