using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class BazsaziResultModel
{
	public static string KeyName => "BazsaziResultModel";

	public ReturnedSaveFuncInfo Result { get; set; }

	public DateTime LastDate { get; set; }

	public static BazsaziResultModel FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\BazsaziResultModel.cs"))
		{
			try
			{
				return data.FromJson<BazsaziResultModel>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\BazsaziResultModel.cs", 17);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\BazsaziResultModel.cs", 21);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\BazsaziResultModel.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\BazsaziResultModel.cs", 34);
				return "";
			}
		}
	}
}
