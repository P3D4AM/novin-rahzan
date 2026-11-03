using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Entities.Accounting.TaxService;

namespace PacketParser.Settings.Accounting;

public class TaxCustomInfo
{
	public const string KeyName = "Acc_TaxCustomInfo";

	public List<WebTaxCustomInfo> Data { get; set; }

	public static TaxCustomInfo FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCustomInfo.cs"))
		{
			try
			{
				return data.FromJson<TaxCustomInfo>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCustomInfo.cs", 18);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCustomInfo.cs", 22);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCustomInfo.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCustomInfo.cs", 35);
				return "";
			}
		}
	}
}
