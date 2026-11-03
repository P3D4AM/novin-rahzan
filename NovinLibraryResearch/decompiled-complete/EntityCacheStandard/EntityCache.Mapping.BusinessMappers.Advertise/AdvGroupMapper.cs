using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class AdvGroupMapper : IItemAsyncMapper<AdvGroupBusiness, AdvGroupBusiness>
{
	public static AdvGroupMapper Instance { get; private set; } = new AdvGroupMapper();


	public Task<AdvGroupBusiness> MapAsync(string contextGuid, AdvGroupBusiness source, AdvGroupBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AdvGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.GroupName = source.GroupName;
			destination.Status = source.Status;
			destination.ParentGuid = source.ParentGuid;
			destination.IsChecked = source.IsChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\AdvGroupMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AdvGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvGroupBusiness> source, List<AdvGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
