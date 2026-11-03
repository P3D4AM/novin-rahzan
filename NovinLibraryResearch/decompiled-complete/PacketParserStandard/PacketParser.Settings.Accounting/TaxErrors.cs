using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;

namespace PacketParser.Settings.Accounting;

public class TaxErrors
{
	public const string KeyName = "Acc_TaxErrors";

	public List<WebTaxErrors> ErrorList { get; set; }

	public static TaxErrors FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxErrors.cs"))
		{
			try
			{
				return data.FromJson<TaxErrors>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxErrors.cs", 18);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxErrors.cs", 22);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxErrors.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxErrors.cs", 35);
				return "";
			}
		}
	}
}
