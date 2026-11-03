using System;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class VersionUpdateSettings
{
	public const string KeyName = "Acc_VersionUpdateSettings";

	public string Version { get; set; }

	public EnDialogResult Resault { get; set; }

	public static VersionUpdateSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\VersionUpdateSettings.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(data))
				{
					return new VersionUpdateSettings();
				}
				return data.FromJson<VersionUpdateSettings>(sendErrorIfException: true, "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\VersionUpdateSettings.cs", 18);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\VersionUpdateSettings.cs", 22);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\VersionUpdateSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\VersionUpdateSettings.cs", 35);
				return "";
			}
		}
	}
}
