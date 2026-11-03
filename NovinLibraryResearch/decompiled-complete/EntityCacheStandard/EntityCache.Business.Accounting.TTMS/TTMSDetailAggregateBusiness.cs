using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.TTMS;

public class TTMSDetailAggregateBusiness
{
	public TTMSDetailAggregateDetailBusiness SellTaxContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness SellTaxContainsService { get; set; }

	public TTMSDetailAggregateDetailBusiness SellTaxNoContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness SellTaxNoContainsService { get; set; }

	public TTMSDetailAggregateDetailBusiness BackSellTaxContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness BackSellTaxNoContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness BuyTaxContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness BuyTaxNoContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness BackBuyTaxContainsProduct { get; set; }

	public TTMSDetailAggregateDetailBusiness BackBuyTaxNoContainsProduct { get; set; }

	public static TTMSDetailAggregateBusiness Calcutale(IEnumerable<OrderTtmsReport> listOrder, CancellationToken token)
	{
		try
		{
			if (listOrder == null || !listOrder.Any() || token.IsCancellationRequested)
			{
				return null;
			}
			TTMSDetailAggregateBusiness tTMSDetailAggregateBusiness = new TTMSDetailAggregateBusiness();
			IEnumerable<OrderTtmsReport> enumerable = listOrder?.Where((OrderTtmsReport q) => q.ProductTax != 0m && q.ProductSumWithTax != 0m && q.FactorType == Document.OrderSell);
			IEnumerable<OrderTtmsReport> enumerable2 = listOrder?.Where((OrderTtmsReport q) => q.ServiceTax != 0m && q.ServiceSumWithTax != 0m && q.FactorType == Document.OrderSell);
			IEnumerable<OrderTtmsReport> enumerable3 = listOrder?.Where((OrderTtmsReport q) => q.ProductSumWithoutTax != 0m && q.FactorType == Document.OrderSell);
			IEnumerable<OrderTtmsReport> enumerable4 = listOrder?.Where((OrderTtmsReport q) => q.ServiceSumWithoutTax != 0m && q.FactorType == Document.OrderSell);
			IEnumerable<OrderTtmsReport> enumerable5 = listOrder?.Where((OrderTtmsReport q) => q.ProductTax != 0m && q.FactorType == Document.OrderBackSell);
			IEnumerable<OrderTtmsReport> enumerable6 = listOrder?.Where((OrderTtmsReport q) => q.FactorType == Document.OrderBackSell);
			IEnumerable<OrderTtmsReport> enumerable7 = listOrder?.Where((OrderTtmsReport q) => q.ProductTax != 0m && q.FactorType == Document.OrderBuy);
			IEnumerable<OrderTtmsReport> enumerable8 = listOrder?.Where((OrderTtmsReport q) => q.FactorType == Document.OrderBuy);
			IEnumerable<OrderTtmsReport> enumerable9 = listOrder?.Where((OrderTtmsReport q) => q.ProductTax != 0m && q.FactorType == Document.OrderBackBuy);
			IEnumerable<OrderTtmsReport> enumerable10 = listOrder?.Where((OrderTtmsReport q) => q.FactorType == Document.OrderBackBuy);
			tTMSDetailAggregateBusiness.SellTaxContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable?.Sum((OrderTtmsReport q) => q.ProductSumWithTax) ?? 0m),
				Tax = (enumerable?.Sum((OrderTtmsReport q) => q.ProductTax) ?? 0m),
				TotalSum = (enumerable?.Sum((OrderTtmsReport q) => q.ProductTotalSumWithTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.SellTaxContainsService = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable2?.Sum((OrderTtmsReport q) => q.ServiceSumWithTax) ?? 0m),
				Tax = (enumerable2?.Sum((OrderTtmsReport q) => q.ServiceTax) ?? 0m),
				TotalSum = (enumerable2?.Sum((OrderTtmsReport q) => q.ServiceTotalSumWithTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.SellTaxNoContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable3?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m),
				Tax = 0m,
				TotalSum = (enumerable3?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.SellTaxNoContainsService = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable4?.Sum((OrderTtmsReport q) => q.ServiceSumWithoutTax) ?? 0m),
				Tax = 0m,
				TotalSum = (enumerable4?.Sum((OrderTtmsReport q) => q.ServiceSumWithoutTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.BackSellTaxContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable5?.Sum((OrderTtmsReport q) => q.ProductSumWithTax) ?? 0m),
				Tax = (enumerable5?.Sum((OrderTtmsReport q) => q.ProductTax) ?? 0m),
				TotalSum = (enumerable5?.Sum((OrderTtmsReport q) => q.ProductTotalSumWithTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.BackSellTaxNoContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable6?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m),
				Tax = 0m,
				TotalSum = (enumerable6?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.BuyTaxContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable7?.Sum((OrderTtmsReport q) => q.ProductSumWithTax) ?? 0m),
				Tax = (enumerable7?.Sum((OrderTtmsReport q) => q.ProductTax) ?? 0m),
				TotalSum = (enumerable7?.Sum((OrderTtmsReport q) => q.ProductTotalSumWithTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.BuyTaxNoContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable8?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m),
				Tax = 0m,
				TotalSum = (enumerable8?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.BackBuyTaxContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable9?.Sum((OrderTtmsReport q) => q.ProductSumWithTax) ?? 0m),
				Tax = (enumerable9?.Sum((OrderTtmsReport q) => q.ProductTax) ?? 0m),
				TotalSum = (enumerable9?.Sum((OrderTtmsReport q) => q.ProductTotalSumWithTax) ?? 0m)
			};
			tTMSDetailAggregateBusiness.BackBuyTaxNoContainsProduct = new TTMSDetailAggregateDetailBusiness
			{
				Sum = (enumerable10?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m),
				Tax = 0m,
				TotalSum = (enumerable10?.Sum((OrderTtmsReport q) => q.ProductSumWithoutTax) ?? 0m)
			};
			return tTMSDetailAggregateBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Calcutale", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TTMS\\TTMSDetailAggregateBusiness.cs", 115);
			throw;
		}
	}
}
