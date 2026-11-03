using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Mapping.BusinessMappers.Accounting.Gift;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.DiscountCoupon;

public class CouponDiscountDetailMapper : IItemAsyncMapper<CouponDiscountDetailBusiness, CouponDiscountDetailBusiness>, IItemAsyncMapper<CouponDiscountDetailBusiness, WebDiscountCouponDetail>, IItemAsyncMapper<WebDiscountCouponDetail, CouponDiscountDetailBusiness>
{
	public static CouponDiscountDetailMapper Instance { get; private set; } = new CouponDiscountDetailMapper();


	public async Task<CouponDiscountDetailBusiness> MapAsync(string contextGuid, CouponDiscountDetailBusiness source, CouponDiscountDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new CouponDiscountDetailBusiness();
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.AmountCoupon = source.AmountCoupon;
			destination.MinBuy = source.MinBuy;
			destination.MaxBuy = source.MaxBuy;
			destination.MaxDiscountPrice = source.MaxDiscountPrice;
			destination.EnDiscountType = source.EnDiscountType;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.GiftGuid = source.GiftGuid;
			destination.CustomerContains = source.CustomerContains;
			destination.ProductContains = source.ProductContains;
			destination.GardeshAffectiveDays = source.GardeshAffectiveDays;
			destination.GardeshStartPrice = source.GardeshStartPrice;
			destination.GardeshEndPrice = source.GardeshEndPrice;
			destination.MinBuyQuantity = source.MinBuyQuantity;
			if ((source?.RelationList?.Any()).GetValueOrDefault())
			{
				await destination.AddEntityListAsync(await CouponDiscountDetailRelationMapper.Instans.MapListAsync(contextGuid, source.RelationList, new List<CouponDiscountEntityRelationBusiness>(), tranName, token), isClearList: true, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountDetailMapper.cs", 58);
			throw;
		}
	}

	public async Task<WebDiscountCouponDetail> MapAsync(string contextGuid, CouponDiscountDetailBusiness source, WebDiscountCouponDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebDiscountCouponDetail();
			}
			destination.Guid = source.Guid;
			destination.AmountCoupon = source.AmountCoupon;
			destination.MinBuy = source.MinBuy;
			destination.MaxBuy = source.MaxBuy;
			destination.MaxDiscountPrice = source.MaxDiscountPrice;
			destination.EnDiscountType = source.EnDiscountType;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.GiftGuid = source.GiftGuid;
			destination.CustomerContains = source.CustomerContains;
			destination.ProductContains = source.ProductContains;
			destination.GardeshAffectiveDays = source.GardeshAffectiveDays;
			destination.GardeshStartPrice = source.GardeshStartPrice;
			destination.GardeshEndPrice = source.GardeshEndPrice;
			destination.MinBuyQuantity = source.MinBuyQuantity;
			if ((source?.RelationList?.Any()).GetValueOrDefault())
			{
				destination.CouponEntityRelation = await CouponDiscountDetailRelationMapper.Instans.MapListAsync(contextGuid, source.RelationList, new List<WebDiscountCouponDetailRelation>(), tranName, token);
			}
			if ((source?.GiftGuid ?? Guid.Empty) != Guid.Empty)
			{
				GiftBusiness giftBusiness = await GiftBusiness.GetAsync(contextGuid, source.GiftGuid.Value, token, tranName);
				if (giftBusiness != null)
				{
					WebDiscountCouponDetail webDiscountCouponDetail = destination;
					webDiscountCouponDetail.Gift = await GiftMapper.Instance.MapAsync(contextGuid, giftBusiness, new WebGift(), tranName, token);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountDetailMapper.cs", 105);
			throw;
		}
	}

	public async Task<CouponDiscountDetailBusiness> MapAsync(string contextGuid, WebDiscountCouponDetail source, CouponDiscountDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new CouponDiscountDetailBusiness();
			}
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
			destination.AmountCoupon = source.AmountCoupon;
			destination.MinBuy = source.MinBuy;
			destination.MaxBuy = source.MaxBuy;
			destination.MaxDiscountPrice = source.MaxDiscountPrice;
			destination.EnDiscountType = source.EnDiscountType;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			destination.GiftGuid = source.GiftGuid;
			destination.CustomerContains = source.CustomerContains;
			destination.ProductContains = source.ProductContains;
			destination.GardeshAffectiveDays = source.GardeshAffectiveDays;
			destination.GardeshStartPrice = source.GardeshStartPrice;
			destination.GardeshEndPrice = source.GardeshEndPrice;
			destination.MinBuyQuantity = source.MinBuyQuantity;
			if ((source?.CouponEntityRelation?.Any()).GetValueOrDefault())
			{
				await destination.AddEntityListAsync(await CouponDiscountDetailRelationMapper.Instans.MapListAsync(contextGuid, source.CouponEntityRelation, new List<CouponDiscountEntityRelationBusiness>(), tranName, token), isClearList: true, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountDetailMapper.cs", 147);
			throw;
		}
	}

	public Task<List<CouponDiscountDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountDetailBusiness> source, List<CouponDiscountDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDiscountCouponDetail>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountDetailBusiness> source, List<WebDiscountCouponDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CouponDiscountDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDiscountCouponDetail> source, List<CouponDiscountDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
