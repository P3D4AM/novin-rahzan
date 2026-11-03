using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class AdvCategoryMapper : IItemAsyncMapper<AdvCategoryBusiness, AdvCategoryBusiness>
{
	public static AdvCategoryMapper Instance { get; private set; } = new AdvCategoryMapper();


	public Task<AdvCategoryBusiness> MapAsync(string contextGuid, AdvCategoryBusiness source, AdvCategoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AdvCategoryBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.ParentGuid = source.ParentGuid;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\AdvCategoryMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AdvCategoryBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvCategoryBusiness> source, List<AdvCategoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
