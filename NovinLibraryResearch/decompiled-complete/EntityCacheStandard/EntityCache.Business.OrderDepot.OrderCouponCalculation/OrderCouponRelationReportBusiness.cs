using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.Selection;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.OrderDepot.OrderCouponCalculation;

public class OrderCouponRelationReportBusiness : IHasContext
{
	private static SemaphoreSlim calculateSemaphore = new SemaphoreSlim(1, 1);

	public Guid CouponGuid { get; set; }

	public string Title { get; set; }

	public DateTime? DateTo { get; set; }

	public decimal OrderSum { get; set; }

	public decimal CouponOrderDiscount { get; set; }

	public decimal CouponDiscount => CouponOrderDiscount + (DetailList?.Sum((OrderCouponRelationDeltailReportBusiness q) => q.Discount) ?? 0m);

	public decimal OrderSumAfterDiscount => OrderSum - CouponDiscount;

	public string ContextGuid { get; set; }

	public bool IsChecked { get; set; }

	public EnCouponType Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay();

	public EnCouponMode Mode { get; set; }

	public Guid? GiftGuid { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public List<OrderCouponRelationDeltailReportBusiness> DetailList { get; private set; }

	public OrderCouponRelationReportBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderCouponRelationReportBusiness.cs", 42);
		}
	}

	public static async Task<List<OrderCouponRelationReportBusiness>> CalculateAsync(string contextGuid, OrderBusiness order, List<Guid> couponGuidList, CancellationToken token, string tranName = "")
	{
		try
		{
			couponGuidList = couponGuidList?.Where((Guid q) => q != Guid.Empty)?.ToList();
			if (!(couponGuidList?.Any() ?? false) || !(order?.ListKala?.Any()).GetValueOrDefault())
			{
				return null;
			}
			if (order.Discount > 0m || order.DiscountRows > 0m)
			{
				return null;
			}
			return (await Task.WhenAll(couponGuidList.Select((Guid q) => CalculateAsync(contextGuid, order, q, token, tranName))))?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderCouponRelationReportBusiness.cs", 57);
			throw;
		}
	}

	private static async Task<OrderCouponRelationReportBusiness> CalculateAsync(string contextGuid, OrderBusiness order, Guid couponGuid, CancellationToken token, string tranName)
	{
		OrderCouponRelationReportBusiness couponCalcResult = new OrderCouponRelationReportBusiness(contextGuid);
		try
		{
			if (couponGuid == Guid.Empty || !(order?.ListKala?.Any()).GetValueOrDefault())
			{
				return null;
			}
			couponCalcResult = await CalculateAsync(contextGuid, order, await CouponDiscountBusiness.GetAsync(contextGuid, couponGuid), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderCouponRelationReportBusiness.cs", 71);
		}
		return couponCalcResult;
	}

	public static async Task<OrderCouponRelationReportBusiness> CalculateAsync(string contextGuid, OrderBusiness order, CouponDiscountBusiness coupon, CancellationToken token, string tranName)
	{
		OrderCouponRelationReportBusiness couponCalcResult = new OrderCouponRelationReportBusiness(contextGuid);
		try
		{
			if (coupon == null || !(order?.ListKala?.Any()).GetValueOrDefault())
			{
				return null;
			}
			if (!(coupon?.CouponDetailList?.Any()).GetValueOrDefault())
			{
				return couponCalcResult;
			}
			couponCalcResult.ContextGuid = contextGuid;
			couponCalcResult.CouponGuid = coupon.Guid;
			couponCalcResult.Title = coupon.Title;
			couponCalcResult.DateTo = coupon.DateTo;
			couponCalcResult.OrderSum = order.TotalSum;
			foreach (CouponDiscountDetailBusiness couponDetail in coupon.CouponDetailList)
			{
				await CalculateAsync(contextGuid, couponDetail, order, couponCalcResult, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderCouponRelationReportBusiness.cs", 91);
		}
		return couponCalcResult;
	}

	private static async Task CalculateAsync(string contextGuid, CouponDiscountDetailBusiness couponDet, OrderBusiness order, OrderCouponRelationReportBusiness couponCalcResult, CancellationToken token, string tranName)
	{
		try
		{
			List<Guid> list = order?.ListKala?.Select((OrderDetailBusiness q) => q.ProductGuid)?.ToList();
			if (list == null || !list.Any())
			{
				return;
			}
			ConcurrentDictionary<Guid, List<Guid>> concurrentDictionary = await ProductGroupRelationBusiness.GetDictionaryAsync(contextGuid, list, token, tranName);
			if (concurrentDictionary == null || !concurrentDictionary.Any())
			{
				return;
			}
			if (couponDet.Master != null)
			{
				couponCalcResult.Type = couponDet.Master.Type;
				couponCalcResult.Mode = couponDet.Master.Mode;
			}
			couponCalcResult.GiftGuid = couponDet.GiftGuid;
			if ((couponDet?.ProductList?.Any()).GetValueOrDefault() && (couponDet?.ProductContains ?? false))
			{
				List<OrderDetailBusiness> list2 = order.ListKala.Where((OrderDetailBusiness q) => couponDet.ProductList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(q.ProductGuid))?.ToList();
				if (list2 != null && list2.Any())
				{
					await ApplyCouponDetOnOrderAsync(list2, order, couponDet, couponCalcResult, token);
				}
				return;
			}
			if ((couponDet?.ProductGroupList?.Any()).GetValueOrDefault() && (couponDet?.ProductContains ?? false))
			{
				List<Guid> lstGuid2 = new List<Guid>();
				IEnumerable<Guid> enumerable = concurrentDictionary.SelectMany((KeyValuePair<Guid, List<Guid>> q) => q.Value).Intersect(couponDet.ProductGroupList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid));
				if (enumerable != null && enumerable.Any())
				{
					foreach (Guid item in enumerable)
					{
						IEnumerable<KeyValuePair<Guid, List<Guid>>> enumerable2 = concurrentDictionary.Where((KeyValuePair<Guid, List<Guid>> q) => q.Value.Contains(item));
						if (enumerable2 != null && enumerable2.Any())
						{
							lstGuid2.AddRange(enumerable2.Select((KeyValuePair<Guid, List<Guid>> q) => q.Key));
						}
					}
				}
				if (lstGuid2?.Any() ?? false)
				{
					List<OrderDetailBusiness> list3 = order.ListKala.Where((OrderDetailBusiness q) => lstGuid2.Contains(q.ProductGuid))?.ToList();
					if (list3 != null && list3.Any())
					{
						await ApplyCouponDetOnOrderAsync(list3, order, couponDet, couponCalcResult, token);
					}
				}
				return;
			}
			if ((couponDet?.ProductList?.Any()).GetValueOrDefault() && !(couponDet?.ProductContains ?? false))
			{
				List<OrderDetailBusiness> list4 = order.ListKala.Where((OrderDetailBusiness q) => !couponDet.ProductList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(q.ProductGuid))?.ToList();
				if (list4 != null && list4.Any())
				{
					await ApplyCouponDetOnOrderAsync(list4, order, couponDet, couponCalcResult, token);
				}
				return;
			}
			if (!(couponDet?.ProductGroupList?.Any()).GetValueOrDefault() || (couponDet?.ProductContains ?? false))
			{
				await ApplyCouponDetOnOrderAsync(order.ListKala.ToList(), order, couponDet, couponCalcResult, token);
				return;
			}
			List<Guid> lstGuid = new List<Guid>();
			foreach (KeyValuePair<Guid, List<Guid>> item2 in concurrentDictionary)
			{
				if (!(item2.Value?.Any() ?? false))
				{
					continue;
				}
				foreach (Guid groupGuid in item2.Value)
				{
					lstGuid.AddRange(order.ListKala.Where((OrderDetailBusiness q) => !couponDet.ProductGroupList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(groupGuid))?.Select((OrderDetailBusiness q) => q.Guid)?.ToList());
				}
			}
			if (lstGuid?.Any() ?? false)
			{
				List<OrderDetailBusiness> list5 = order.ListKala.Where((OrderDetailBusiness q) => lstGuid.Contains(q.Guid))?.ToList();
				if (list5 != null && list5.Any())
				{
					await ApplyCouponDetOnOrderAsync(list5, order, couponDet, couponCalcResult, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderCouponRelationReportBusiness.cs", 159);
		}
	}

	private static async Task ApplyCouponDetOnOrderAsync(List<OrderDetailBusiness> validProducts, OrderBusiness order, CouponDiscountDetailBusiness couponDet, OrderCouponRelationReportBusiness result, CancellationToken token)
	{
		_ = 3;
		try
		{
			if (result == null || couponDet == null || !(validProducts?.Any() ?? false) || token.IsCancellationRequested)
			{
				return;
			}
			if (result.DetailList == null)
			{
				result.DetailList = new List<OrderCouponRelationDeltailReportBusiness>();
			}
			switch (couponDet.EnDiscountType)
			{
			case EnDiscountType.OrderFixedPercent:
			{
				decimal num = order.TotalSum - order.TotalTax - order.Avarez - order.Discount;
				decimal discount = Math.Round(num * couponDet.AmountCoupon / 100m, result.DigitForRound);
				if (couponDet.MaxDiscountPrice != 0m && discount > couponDet.MaxDiscountPrice)
				{
					discount = couponDet.MaxDiscountPrice;
				}
				if (discount > order.TotalSum)
				{
					discount = order.TotalSum;
				}
				try
				{
					await calculateSemaphore.WaitAsync(token);
					result.CouponOrderDiscount += discount;
					foreach (OrderDetailBusiness validProduct in validProducts)
					{
						result.DetailList.Add(new OrderCouponRelationDeltailReportBusiness(couponDet.Guid, validProduct.Guid, 0m));
					}
					break;
				}
				finally
				{
					calculateSemaphore.Release();
				}
			}
			case EnDiscountType.OrderFixedPrice:
			{
				decimal discount = couponDet.AmountCoupon;
				if (couponDet.MaxDiscountPrice != 0m && discount > couponDet.MaxDiscountPrice)
				{
					discount = couponDet.MaxDiscountPrice;
				}
				if (discount > order.TotalSum)
				{
					discount = order.TotalSum;
				}
				try
				{
					await calculateSemaphore.WaitAsync(token);
					result.CouponOrderDiscount += discount;
					foreach (OrderDetailBusiness validProduct2 in validProducts)
					{
						result.DetailList.Add(new OrderCouponRelationDeltailReportBusiness(couponDet.Guid, validProduct2.Guid, 0m));
					}
					break;
				}
				finally
				{
					calculateSemaphore.Release();
				}
			}
			case EnDiscountType.ProductFixedPercent:
			{
				foreach (OrderDetailBusiness orderDet in validProducts)
				{
					decimal discount = Math.Round((orderDet.Price * orderDet.Quantity - orderDet.Discount) * couponDet.AmountCoupon / 100m, result.DigitForRound);
					if (couponDet.MaxDiscountPrice != 0m && discount > couponDet.MaxDiscountPrice)
					{
						discount = couponDet.MaxDiscountPrice;
					}
					if (discount > order.TotalSum)
					{
						discount = order.TotalSum;
					}
					try
					{
						await calculateSemaphore.WaitAsync(token);
						result.DetailList.Add(new OrderCouponRelationDeltailReportBusiness(couponDet.Guid, orderDet.Guid, discount));
					}
					finally
					{
						calculateSemaphore.Release();
					}
				}
				break;
			}
			case EnDiscountType.ProductFixedPrice:
			{
				foreach (OrderDetailBusiness orderDet in validProducts)
				{
					decimal discount = couponDet.AmountCoupon;
					if (couponDet.MaxDiscountPrice != 0m && discount > couponDet.MaxDiscountPrice)
					{
						discount = couponDet.MaxDiscountPrice;
					}
					if (discount > order.TotalSum)
					{
						discount = order.TotalSum;
					}
					try
					{
						await calculateSemaphore.WaitAsync(token);
						result.DetailList.Add(new OrderCouponRelationDeltailReportBusiness(couponDet.Guid, orderDet.Guid, discount));
					}
					finally
					{
						calculateSemaphore.Release();
					}
				}
				break;
			}
			default:
				throw new NotImplementedException();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyCouponDetOnOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderCouponCalculation\\OrderCouponRelationReportBusiness.cs", 236);
			throw;
		}
	}
}
