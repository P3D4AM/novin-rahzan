using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ProductPriceUpdateMapper : IItemAsyncMapper<ProductPriceUpdateBusiness, ProductPriceUpdateBusiness>
{
	public static ProductPriceUpdateMapper Instance { get; private set; } = new ProductPriceUpdateMapper();


	public Task<ProductPriceUpdateBusiness> MapAsync(string contextGuid, ProductPriceUpdateBusiness source, ProductPriceUpdateBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductPriceUpdateBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.DateM = source.DateM;
			destination.Status = source.Status;
			destination.RegularPrice = source.RegularPrice;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductPriceUpdateMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ProductPriceUpdateBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductPriceUpdateBusiness> source, List<ProductPriceUpdateBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
