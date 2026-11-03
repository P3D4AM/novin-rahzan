using System;
using PacketParser.Entities;
using PacketParser.Settings.Accounting.AbonmanSettings;

namespace PacketParser.Settings.Accounting;

public class ActivationAbonmanSettings
{
	public const string KeyName = "Acc_AbonmanSettings";

	public DateTime Modified { get; set; }

	public int Version { get; set; }

	public PosAbonman PosAbonman { get; set; } = new PosAbonman();


	public TariffPackageSetting TariffPackageSetting { get; set; } = new TariffPackageSetting();


	public AbonmanPackageSetting AbonmanPackageSetting { get; set; } = new AbonmanPackageSetting();


	public static ActivationAbonmanSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AbonmanSettings\\ActivationAbonmanSettings.cs"))
		{
			try
			{
				return data.FromJson<ActivationAbonmanSettings>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AbonmanSettings\\ActivationAbonmanSettings.cs", 25);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AbonmanSettings\\ActivationAbonmanSettings.cs", 29);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AbonmanSettings\\ActivationAbonmanSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\AbonmanSettings\\ActivationAbonmanSettings.cs", 42);
				return "";
			}
		}
	}
}
