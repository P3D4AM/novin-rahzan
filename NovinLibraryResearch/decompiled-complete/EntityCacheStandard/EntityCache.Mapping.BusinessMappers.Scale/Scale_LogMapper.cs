using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_LogMapper : IItemAsyncMapper<Scale_LogBusiness, Scale_LogBusiness>
{
	public static Scale_LogMapper Instance { get; private set; } = new Scale_LogMapper();


	public Task<Scale_LogBusiness> MapAsync(string contextGuid, Scale_LogBusiness source, Scale_LogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_LogBusiness(contextGuid);
			}
			destination.DateM = source.DateM;
			destination.Guid = source.Guid;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_LogMapper.cs", 32);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_LogBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_LogBusiness> source, List<Scale_LogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
