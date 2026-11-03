using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Selection;

public class ProductGroupRelationMapper : IItemAsyncMapper<ProductGroupRelationBusiness, ProductGroupRelationBusiness>
{
	public static ProductGroupRelationMapper Instance { get; private set; } = new ProductGroupRelationMapper();


	public Task<ProductGroupRelationBusiness> MapAsync(string contextGuid, ProductGroupRelationBusiness source, ProductGroupRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductGroupRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.GroupGuid = source.GroupGuid;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\ProductGroupRelationMapper.cs", 33);
			throw;
		}
	}

	public Task<List<ProductGroupRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductGroupRelationBusiness> source, List<ProductGroupRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
