using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class RegionMapper : IItemAsyncMapper<RegionBusiness, RegionBusiness>
{
	public static RegionMapper Instance { get; private set; } = new RegionMapper();


	public Task<RegionBusiness> MapAsync(string contextGuid, RegionBusiness source, RegionBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RegionBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.CityGuid = source.CityGuid;
			destination.Name = source.Name;
			destination.Type = source.Type;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\RegionMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<RegionBusiness>> MapListAsync(string contextGuid, IEnumerable<RegionBusiness> source, List<RegionBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
