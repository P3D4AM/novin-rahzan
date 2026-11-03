using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_PrdVahedMapper : IItemAsyncMapper<Scale_PrdVahedBusiness, Scale_PrdVahedBusiness>
{
	public static Scale_PrdVahedMapper Instance { get; private set; } = new Scale_PrdVahedMapper();


	public Task<Scale_PrdVahedBusiness> MapAsync(string contextGuid, Scale_PrdVahedBusiness source, Scale_PrdVahedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_PrdVahedBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_PrdVahedMapper.cs", 33);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_PrdVahedBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_PrdVahedBusiness> source, List<Scale_PrdVahedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
