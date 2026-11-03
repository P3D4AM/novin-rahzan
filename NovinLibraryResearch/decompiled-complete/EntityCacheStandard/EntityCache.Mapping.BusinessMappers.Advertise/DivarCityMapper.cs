using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class DivarCityMapper : IItemAsyncMapper<DivarCityBusiness, DivarCityBusiness>
{
	public static DivarCityMapper Instance { get; private set; } = new DivarCityMapper();


	public Task<DivarCityBusiness> MapAsync(string contextGuid, DivarCityBusiness source, DivarCityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DivarCityBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CityName = source.CityName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\DivarCityMapper.cs", 32);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<DivarCityBusiness>> MapListAsync(string contextGuid, IEnumerable<DivarCityBusiness> source, List<DivarCityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
