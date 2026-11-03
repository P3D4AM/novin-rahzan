using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class StateMapper : IItemAsyncMapper<StateBusiness, StateBusiness>
{
	public static StateMapper Instance { get; private set; } = new StateMapper();


	public Task<StateBusiness> MapAsync(string contextGuid, StateBusiness source, StateBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new StateBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.StateName = source.StateName;
			destination.isChecked = source.isChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\StateMapper.cs", 33);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<StateBusiness>> MapListAsync(string contextGuid, IEnumerable<StateBusiness> source, List<StateBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
