using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_CommandMapper : IItemAsyncMapper<Scale_CommandBusiness, Scale_CommandBusiness>
{
	public static Scale_CommandMapper Instance { get; private set; } = new Scale_CommandMapper();


	public Task<Scale_CommandBusiness> MapAsync(string contextGuid, Scale_CommandBusiness source, Scale_CommandBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_CommandBusiness(contextGuid);
			}
			destination.DateM = source.DateM;
			destination.DateSH = source.DateSH;
			destination.Command = source.Command;
			destination.Sender = source.Sender;
			destination.ScaleGuid = source.ScaleGuid;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_CommandMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_CommandBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_CommandBusiness> source, List<Scale_CommandBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
