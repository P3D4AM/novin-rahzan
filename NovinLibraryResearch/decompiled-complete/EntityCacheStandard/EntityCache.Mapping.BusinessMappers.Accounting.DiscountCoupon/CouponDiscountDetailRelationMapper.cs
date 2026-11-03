using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.DiscountCoupon;

public class CouponDiscountDetailRelationMapper : IItemAsyncMapper<CouponDiscountEntityRelationBusiness, WebDiscountCouponDetailRelation>, IItemAsyncMapper<CouponDiscountEntityRelationBusiness, CouponDiscountEntityRelationBusiness>, IItemAsyncMapper<WebDiscountCouponDetailRelation, CouponDiscountEntityRelationBusiness>
{
	public static CouponDiscountDetailRelationMapper Instans { get; private set; } = new CouponDiscountDetailRelationMapper();


	public Task<WebDiscountCouponDetailRelation> MapAsync(string contextGuid, CouponDiscountEntityRelationBusiness source, WebDiscountCouponDetailRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDiscountCouponDetailRelation();
			}
			destination.EntityGuid = source.EntityGuid;
			destination.CouponDetailGuid = source.CouponDetailGuid;
			destination.EntityType = source.EntityType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountDetailRelationMapper.cs", 35);
			throw;
		}
	}

	public Task<CouponDiscountEntityRelationBusiness> MapAsync(string contextGuid, CouponDiscountEntityRelationBusiness source, CouponDiscountEntityRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CouponDiscountEntityRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.EntityGuid = source.EntityGuid;
			destination.CouponDetailGuid = source.CouponDetailGuid;
			destination.EntityType = source.EntityType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountDetailRelationMapper.cs", 57);
			throw;
		}
	}

	public Task<CouponDiscountEntityRelationBusiness> MapAsync(string contextGuid, WebDiscountCouponDetailRelation source, CouponDiscountEntityRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CouponDiscountEntityRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.EntityGuid = source.EntityGuid;
			destination.CouponDetailGuid = source.CouponDetailGuid;
			destination.EntityType = source.EntityType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DiscountCoupon\\CouponDiscountDetailRelationMapper.cs", 79);
			throw;
		}
	}

	public Task<List<WebDiscountCouponDetailRelation>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountEntityRelationBusiness> source, List<WebDiscountCouponDetailRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CouponDiscountEntityRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountEntityRelationBusiness> source, List<CouponDiscountEntityRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CouponDiscountEntityRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDiscountCouponDetailRelation> source, List<CouponDiscountEntityRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
