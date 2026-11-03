using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ProductDecimalRelationMapper : IItemAsyncMapper<ProductDecimalRelationBusiness, ProductDecimalRelationBusiness>
{
	public static ProductDecimalRelationMapper Instance { get; private set; } = new ProductDecimalRelationMapper();


	public Task<ProductDecimalRelationBusiness> MapAsync(string contextGuid, ProductDecimalRelationBusiness source, ProductDecimalRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductDecimalRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Amount = source.Amount;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductDecimalRelationMapper.cs", 32);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ProductDecimalRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductDecimalRelationBusiness> source, List<ProductDecimalRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapListAsync(contextGuid, source, destination, tranName);
	}
}
