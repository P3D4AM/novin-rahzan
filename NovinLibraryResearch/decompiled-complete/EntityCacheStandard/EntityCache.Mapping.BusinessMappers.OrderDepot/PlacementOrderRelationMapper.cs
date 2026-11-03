using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class PlacementOrderRelationMapper : IItemAsyncMapper<PlacementOrderRelationBusiness, PlacementOrderRelationBusiness>
{
	public static PlacementOrderRelationMapper Instance { get; private set; } = new PlacementOrderRelationMapper();


	public Task<PlacementOrderRelationBusiness> MapAsync(string contextGuid, PlacementOrderRelationBusiness source, PlacementOrderRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PlacementOrderRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.PlacementGuid = source.PlacementGuid;
			destination.OrderGuid = source.OrderGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OpenInvoices\\PlacementOrderRelationMapper.cs", 32);
			throw;
		}
	}

	public Task<List<PlacementOrderRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<PlacementOrderRelationBusiness> source, List<PlacementOrderRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
