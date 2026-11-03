using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class TaxUnits
{
	public const string KeyName = "Acc_TaxUnits";

	public List<WebTaxUnit> UnitList { get; set; }

	public static TaxUnits FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxUnits.cs"))
		{
			try
			{
				return data.FromJson<TaxUnits>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxUnits.cs", 19);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxUnits.cs", 23);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxUnits.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxUnits.cs", 36);
				return "";
			}
		}
	}
}
