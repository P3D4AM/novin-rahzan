using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Settings.Global;

public class ShabaNumberSetting
{
	public const string KeyName = "Acc_ShabaNumberSetting";

	public List<WebShabaTemplate> Templates { get; set; }

	public static ShabaNumberSetting FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ShabaNumberSetting.cs"))
		{
			try
			{
				return data.FromJson<ShabaNumberSetting>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ShabaNumberSetting.cs", 18);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ShabaNumberSetting.cs", 22);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ShabaNumberSetting.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ShabaNumberSetting.cs", 35);
				return "";
			}
		}
	}

	public static ShabaNumberSetting GetDefaults()
	{
		try
		{
			return new ShabaNumberSetting();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDefaults", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ShabaNumberSetting.cs", 49);
			throw;
		}
	}
}
