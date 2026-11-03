using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Bill;

public class PlacementMapper : IItemAsyncMapper<PlacementBusiness, PlacementBusiness>
{
	public static PlacementMapper Instance { get; private set; } = new PlacementMapper();


	public async Task<PlacementBusiness> MapAsync(string contextGuid, PlacementBusiness source, PlacementBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new PlacementBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.MasterGuid = source.MasterGuid;
			PlacementBusiness placementBusiness = destination;
			placementBusiness.SetNumber(await source.GetNumberAsync(EnSystem.Mapper));
			destination.GroupGuid = source.GroupGuid;
			destination.ItemCount = source.ItemCount;
			destination.Price = source.Price;
			destination.ExtraPrice = source.ExtraPrice;
			destination.Title = source.Title;
			destination.Description = source.Description;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\PlacementMapper.cs", 41);
			throw;
		}
	}

	public Task<List<PlacementBusiness>> MapListAsync(string contextGuid, IEnumerable<PlacementBusiness> source, List<PlacementBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
