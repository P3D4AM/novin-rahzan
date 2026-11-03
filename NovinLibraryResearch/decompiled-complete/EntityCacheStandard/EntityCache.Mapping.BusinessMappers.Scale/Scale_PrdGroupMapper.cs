using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_PrdGroupMapper : IItemAsyncMapper<Scale_PrdGroupBusiness, Scale_PrdGroupBusiness>
{
	public static Scale_PrdGroupMapper Instance { get; private set; } = new Scale_PrdGroupMapper();


	public Task<Scale_PrdGroupBusiness> MapAsync(string contextGuid, Scale_PrdGroupBusiness source, Scale_PrdGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_PrdGroupBusiness(contextGuid);
			}
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_PrdGroupMapper.cs", 33);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_PrdGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_PrdGroupBusiness> source, List<Scale_PrdGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
