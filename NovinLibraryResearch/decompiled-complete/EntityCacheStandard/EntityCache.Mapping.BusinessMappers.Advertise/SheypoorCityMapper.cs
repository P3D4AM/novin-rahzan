using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class SheypoorCityMapper : IItemAsyncMapper<SheypoorCityBusiness, SheypoorCityBusiness>
{
	public static SheypoorCityMapper Instance { get; private set; } = new SheypoorCityMapper();


	public Task<SheypoorCityBusiness> MapAsync(string contextGuid, SheypoorCityBusiness source, SheypoorCityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SheypoorCityBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CityName = source.CityName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\SheypoorCityMapper.cs", 32);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SheypoorCityBusiness>> MapListAsync(string contextGuid, IEnumerable<SheypoorCityBusiness> source, List<SheypoorCityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
