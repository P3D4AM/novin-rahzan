using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.DiscountCoupon;

public class CouponDiscountUsagesMapper : IItemAsyncMapper<CouponDiscountUsagesBusiness, WebCouponDiscountUsages>, IItemAsyncMapper<CouponDiscountUsagesBusiness, CouponDiscountUsagesBusiness>, IItemAsyncMapper<WebCouponDiscountUsages, CouponDiscountUsagesBusiness>
{
	public static CouponDiscountUsagesMapper Instans { get; private set; } = new CouponDiscountUsagesMapper();


	public Task<WebCouponDiscountUsages> MapAsync(string contextGuid, CouponDiscountUsagesBusiness source, WebCouponDiscountUsages destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebCouponDiscountUsages();
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.CouponGuid = source.CouponGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountUsagesMapper.cs", 34);
			throw;
		}
	}

	public Task<CouponDiscountUsagesBusiness> MapAsync(string contextGuid, CouponDiscountUsagesBusiness source, CouponDiscountUsagesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new CouponDiscountUsagesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CouponGuid = source.CouponGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountUsagesMapper.cs", 55);
			throw;
		}
	}

	public Task<CouponDiscountUsagesBusiness> MapAsync(string contextGuid, WebCouponDiscountUsages source, CouponDiscountUsagesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new CouponDiscountUsagesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CouponGuid = source.CouponGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountUsagesMapper.cs", 76);
			throw;
		}
	}

	public Task<List<WebCouponDiscountUsages>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountUsagesBusiness> source, List<WebCouponDiscountUsages> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CouponDiscountUsagesBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountUsagesBusiness> source, List<CouponDiscountUsagesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CouponDiscountUsagesBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCouponDiscountUsages> source, List<CouponDiscountUsagesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
