using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class CityMapper : IItemAsyncMapper<CityBusiness, CityBusiness>
{
	public static CityMapper Instance { get; private set; } = new CityMapper();


	public Task<CityBusiness> MapAsync(string contextGuid, CityBusiness source, CityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CityBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CityName = source.CityName;
			destination.StateGuid = source.StateGuid;
			destination.Weight = source.Weight;
			destination.isDivarCity = source.isDivarCity;
			destination.isSheypoorCity = source.isSheypoorCity;
			destination.isNiazKadeCity = source.isNiazKadeCity;
			destination.isNiazmandyHaCity = source.isNiazmandyHaCity;
			destination.StartPoint = source.StartPoint;
			destination.IsChecked = source.IsChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\CityMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CityBusiness>> MapListAsync(string contextGuid, IEnumerable<CityBusiness> source, List<CityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
