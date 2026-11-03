using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.TozieList;

public class TozieListTasvieReportBusiness
{
	public Guid TafsilGuid { get; private set; }

	public string TafsilName { get; set; }

	public List<int> SellId2List { get; private set; }

	public string SellId2Str => string.Join(",", SellId2List);

	public decimal SellSum { get; private set; }

	public decimal SellDiscount { get; private set; }

	public List<int> BackSellId2List { get; private set; }

	public string BackSellId2Str => string.Join(",", BackSellId2List);

	public decimal BackSellSum { get; private set; }

	public List<int> ReceptionId2List { get; private set; }

	public string ReceptionId2Str => string.Join(",", ReceptionId2List);

	public decimal ReceptionSum { get; private set; }

	public decimal ReceptionSumNaghd { get; private set; }

	public decimal ReceptionSumHavale { get; private set; }

	public decimal ReceptionSumCheck { get; private set; }

	public decimal ReceptionSumEnteghal { get; private set; }

	public decimal Motalebat => SellSum - ReceptionSum - BackSellSum;

	private TozieListTasvieReportBusiness(string contextGuid, Guid tafsilGuid, List<ReceptionReportBusiness> recipts, List<OrderReportBusiness> orders)
	{
		try
		{
			TafsilGuid = tafsilGuid;
			TafsilName = TafsilBusiness.Get(contextGuid, TafsilGuid)?.Name ?? "";
			SellId2List = orders?.Where((OrderReportBusiness q) => q.FactorType == Document.OrderSell)?.Select((OrderReportBusiness q) => q.Id2)?.ToList();
			SellSum = (orders?.Where((OrderReportBusiness q) => q.FactorType == Document.OrderSell)?.Sum((OrderReportBusiness q) => q.TotalSum)).GetValueOrDefault();
			SellDiscount = (orders?.Where((OrderReportBusiness q) => q.FactorType == Document.OrderSell)?.Sum((OrderReportBusiness q) => q.Discount)).GetValueOrDefault();
			BackSellId2List = orders?.Where((OrderReportBusiness q) => q.FactorType == Document.OrderBackSell)?.Select((OrderReportBusiness q) => q.Id2)?.ToList();
			BackSellSum = (orders?.Where((OrderReportBusiness q) => q.FactorType == Document.OrderBackSell)?.Sum((OrderReportBusiness q) => q.TotalSum)).GetValueOrDefault();
			ReceptionId2List = recipts?.Select((ReceptionReportBusiness q) => q.Id2)?.ToList();
			ReceptionSum = recipts?.Sum((ReceptionReportBusiness p) => p.Sum) ?? 0m;
			ReceptionSumNaghd = recipts?.Sum((ReceptionReportBusiness q) => q.SumNaghd) ?? 0m;
			ReceptionSumHavale = recipts?.Sum((ReceptionReportBusiness q) => q.SumHavale) ?? 0m;
			ReceptionSumCheck = recipts?.Sum((ReceptionReportBusiness q) => q.SumChequeSh) ?? 0m;
			ReceptionSumEnteghal = recipts?.Sum((ReceptionReportBusiness q) => q.SumEnteghal) ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListTasvieReportBusiness.cs", 29);
		}
	}

	public static List<TozieListTasvieReportBusiness> GetAll(string contextGuid, IEnumerable<ReceptionReportBusiness> recipts, IEnumerable<OrderReportBusiness> orders)
	{
		List<TozieListTasvieReportBusiness> result = null;
		try
		{
			List<Guid> obj = orders?.Select((OrderReportBusiness p) => p.TafsilGuid)?.ToList() ?? new List<Guid>();
			obj.AddRange(recipts?.Select((ReceptionReportBusiness p) => p.TafsilGuid)?.ToList() ?? new List<Guid>());
			result = (from p in obj.Distinct().ToList()
				select new TozieListTasvieReportBusiness(contextGuid, p, recipts?.Where((ReceptionReportBusiness r) => r.TafsilGuid == p)?.ToList() ?? new List<ReceptionReportBusiness>(), orders?.Where((OrderReportBusiness r) => r.TafsilGuid == p)?.ToList() ?? new List<OrderReportBusiness>()))?.ToList() ?? new List<TozieListTasvieReportBusiness>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListTasvieReportBusiness.cs", 68);
		}
		return result;
	}
}
