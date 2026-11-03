using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_PRDMapper : IItemAsyncMapper<Scale_PRDBusiness, Scale_PRDBusiness>
{
	public static Scale_PRDMapper Instance { get; private set; } = new Scale_PRDMapper();


	public Task<Scale_PRDBusiness> MapAsync(string contextGuid, Scale_PRDBusiness source, Scale_PRDBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_PRDBusiness(contextGuid);
			}
			destination.MemoryNumber = source.MemoryNumber;
			destination.MemoryName = source.MemoryName;
			destination.MemoryPrice = source.MemoryPrice;
			destination.Vazni = source.Vazni;
			destination.TotalQT = source.TotalQT;
			destination.TotalPrice = source.TotalPrice;
			destination.ProductGuid = source.ProductGuid;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_PRDMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_PRDBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_PRDBusiness> source, List<Scale_PRDBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
