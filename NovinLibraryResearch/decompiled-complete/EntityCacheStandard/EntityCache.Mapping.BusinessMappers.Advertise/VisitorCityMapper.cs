using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class VisitorCityMapper : IItemAsyncMapper<VisitorCityBusiness, VisitorCityBusiness>
{
	public static VisitorCityMapper Instance { get; private set; } = new VisitorCityMapper();


	public Task<VisitorCityBusiness> MapAsync(string contextGuid, VisitorCityBusiness source, VisitorCityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VisitorCityBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.VisitorGuid = source.VisitorGuid;
			destination.CityGuid = source.CityGuid;
			destination.IsChecked = source.IsChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\VisitorCityMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<VisitorCityBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorCityBusiness> source, List<VisitorCityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
