using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Entities.Accounting.TaxService;

namespace PacketParser.Settings.Accounting;

public class TaxCurrency
{
	public const string KeyName = "Acc_TaxCurrency";

	public List<WebTaxCurrency> Data { get; set; }

	public static TaxCurrency FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCurrency.cs"))
		{
			try
			{
				return data.FromJson<TaxCurrency>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCurrency.cs", 18);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCurrency.cs", 22);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCurrency.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TaxService\\TaxCurrency.cs", 35);
				return "";
			}
		}
	}
}
