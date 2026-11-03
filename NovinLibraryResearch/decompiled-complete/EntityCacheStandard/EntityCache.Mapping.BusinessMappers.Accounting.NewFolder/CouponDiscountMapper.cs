using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Mapping.BusinessMappers.Accounting.DiscountCoupon;
using PacketParser.Entities;
using PacketParser.Entities.Accounting.DiscountCoupon;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.NewFolder;

public class CouponDiscountMapper : IItemAsyncMapper<CouponDiscountBusiness, CouponDiscountBusiness>, IItemAsyncMapper<CouponDiscountBusiness, WebDiscountCoupon>, IItemAsyncMapper<WebDiscountCoupon, CouponDiscountBusiness>
{
	public static CouponDiscountMapper Instance { get; private set; } = new CouponDiscountMapper();


	public async Task<CouponDiscountBusiness> MapAsync(string contextGuid, CouponDiscountBusiness source, CouponDiscountBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CouponDiscountBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.SingleUse = source.SingleUse;
			destination.CountCouponUse = source.CountCouponUse;
			destination.CountCouponUseForUser = source.CountCouponUseForUser;
			destination.DeliveryId = source.DeliveryId;
			destination.Title = source.Title;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Modified = source.Modified;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Status = source.Status;
			destination.Guid = source.Guid;
			destination.ServerStatus = source.ServerStatus;
			destination.CashierGuid = source.CashierGuid;
			destination.Type = source.Type;
			destination.Mode = source.Mode;
			destination.DeadLineForUse = source.DeadLineForUse;
			destination.MetaData = source.MetaData;
			destination.IsImmediateUseAllowed = source.IsImmediateUseAllowed;
			destination.Version = source.Version;
			CouponDiscountBusiness couponDiscountBusiness = destination;
			couponDiscountBusiness.UsedCount = await source.GetUsedCountAsync(token, tranName);
			if (source.CouponDetailList?.Any() ?? false)
			{
				await destination.AddCouponDetailListAsync(await CouponDiscountDetailMapper.Instance.MapListAsync(contextGuid, source.CouponDetailList, new List<CouponDiscountDetailBusiness>(), tranName, token), token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountMapper.cs", 64);
			throw;
		}
	}

	public async Task<CouponDiscountBusiness> MapAsync(string contextGuid, WebDiscountCoupon source, CouponDiscountBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CouponDiscountBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.SingleUse = source.SingleUse;
			destination.CountCouponUse = source.CountCouponUse;
			destination.CountCouponUseForUser = source.CountCouponUseForUser;
			destination.DeliveryId = source.DeliveryId;
			destination.Title = source.Title;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Modified = source.Modified;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Status = source.Status;
			destination.Guid = source.Guid;
			destination.ServerStatus = source.ServerStatus;
			destination.CashierGuid = source.CashierGuid;
			destination.Type = source.Type;
			destination.Mode = source.Mode;
			destination.DeadLineForUse = source.DeadLineForUse;
			destination.MetaData = source.MetaData;
			destination.IsImmediateUseAllowed = source.IsImmediateUseAllowed;
			destination.Version = source.Version;
			destination.UsedCount = source.UsedCount;
			if (source.DetailList?.Any() ?? false)
			{
				await destination.AddCouponDetailListAsync(await CouponDiscountDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<CouponDiscountDetailBusiness>(), tranName, token), token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountMapper.cs", 113);
			throw;
		}
	}

	public async Task<WebDiscountCoupon> MapAsync(string contextGuid, CouponDiscountBusiness source, WebDiscountCoupon destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebDiscountCoupon();
			}
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.SingleUse = source.SingleUse;
			destination.CountCouponUse = source.CountCouponUse;
			destination.CountCouponUseForUser = source.CountCouponUseForUser;
			destination.DeliveryId = source.DeliveryId;
			destination.Title = source.Title;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Modified = source.Modified;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Status = source.Status;
			destination.Guid = source.Guid;
			destination.ServerStatus = source.ServerStatus;
			destination.CashierGuid = source.CashierGuid;
			destination.Type = source.Type;
			destination.Mode = source.Mode;
			destination.DeadLineForUse = source.DeadLineForUse;
			destination.MetaData = source.MetaData;
			destination.IsImmediateUseAllowed = source.IsImmediateUseAllowed;
			destination.Version = source.Version;
			WebDiscountCoupon webDiscountCoupon = destination;
			webDiscountCoupon.UsedCount = await source.GetUsedCountAsync(token, tranName);
			if (source.CouponDetailList?.Any() ?? false)
			{
				webDiscountCoupon = destination;
				webDiscountCoupon.DetailList = await CouponDiscountDetailMapper.Instance.MapListAsync(contextGuid, source.CouponDetailList, new List<WebDiscountCouponDetail>(), tranName, token);
			}
			IEnumerable<CouponDiscountUsagesBusiness> enumerable = await source.GetAllUagesAsync(token, tranName);
			if (enumerable != null && enumerable.Any())
			{
				WebDiscountCoupon webDiscountCoupon2 = destination;
				if (webDiscountCoupon2.MetaData == null)
				{
					webDiscountCoupon2.MetaData = new CouponDiscountMetaData();
				}
				CouponDiscountMetaData metaData = destination.MetaData;
				metaData.Usages = await CouponDiscountUsagesMapper.Instans.MapListAsync(contextGuid, enumerable, new List<WebCouponDiscountUsages>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountMapper.cs", 165);
			throw;
		}
	}

	public Task<List<CouponDiscountBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountBusiness> source, List<CouponDiscountBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CouponDiscountBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDiscountCoupon> source, List<CouponDiscountBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDiscountCoupon>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountBusiness> source, List<WebDiscountCoupon> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
