using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

public class PlacementFacilitiesRelationMapper : IItemAsyncMapper<PlacementFacilitiesRelationBusiness, PlacementFacilitiesRelationBusiness>
{
	public static PlacementFacilitiesRelationMapper Instance { get; private set; } = new PlacementFacilitiesRelationMapper();


	public Task<PlacementFacilitiesRelationBusiness> MapAsync(string contextGuid, PlacementFacilitiesRelationBusiness source, PlacementFacilitiesRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PlacementFacilitiesRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.FacilitiesGuid = source.FacilitiesGuid;
			destination.PlacementGuid = source.PlacementGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\PlacementFacilitiesRelationMapper.cs", 32);
			throw;
		}
	}

	public Task<List<PlacementFacilitiesRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<PlacementFacilitiesRelationBusiness> source, List<PlacementFacilitiesRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
