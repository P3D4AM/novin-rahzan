using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class VisitorAdvMapper : IItemAsyncMapper<VisitorAdvBusiness, VisitorAdvBusiness>
{
	public static VisitorAdvMapper Instance { get; private set; } = new VisitorAdvMapper();


	public Task<VisitorAdvBusiness> MapAsync(string contextGuid, VisitorAdvBusiness source, VisitorAdvBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VisitorAdvBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.VisitorGuid = source.VisitorGuid;
			destination.AdvGuid = source.AdvGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\VisitorAdvMapper.cs", 33);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<VisitorAdvBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorAdvBusiness> source, List<VisitorAdvBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
