using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OrderCouponCalculation;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OrderDetailCouponCalculationMapper : IItemAsyncMapper<OrderDetailCouponCalculationBusiness, OrderDetailCouponCalculationBusiness>
{
	public static OrderDetailCouponCalculationMapper Instance { get; private set; } = new OrderDetailCouponCalculationMapper();


	public Task<OrderDetailCouponCalculationBusiness> MapAsync(string contextGuid, OrderDetailCouponCalculationBusiness source, OrderDetailCouponCalculationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OrderDetailCouponCalculationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.ProductGuid = source.ProductGuid;
			destination.Quantity = source.Quantity;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Modified = source.Modified;
			destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
			destination.RowSum = source.RowSum;
			destination.RefrencedId = source.RefrencedId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderDetailCouponCalculationMapper.cs", 38);
			throw;
		}
	}

	public Task<List<OrderDetailCouponCalculationBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailCouponCalculationBusiness> source, List<OrderDetailCouponCalculationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
