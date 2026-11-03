using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Snifts;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Snift;

public class OrderSniftMapper : IItemAsyncMapper<OrderDetailBusiness, WebOrderSnifts>
{
	public static OrderSniftMapper Instance { get; private set; } = new OrderSniftMapper();


	public Task<WebOrderSnifts> MapAsync(string contextGuid, OrderDetailBusiness source, WebOrderSnifts destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebOrderSnifts();
			}
			destination.LastOrderDate = source.MasterOrder?.GetDate();
			destination.LastBuyPrice = source.Price;
			destination.LastBuyTaxPercent = source.TaxPercent;
			destination.LastBuyAvarezPercent = source.AvarezPercent;
			destination.LastBuyDiscountPercent = source.DiscountPercent;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Snift\\OrderSniftMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebOrderSnifts>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<WebOrderSnifts> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
