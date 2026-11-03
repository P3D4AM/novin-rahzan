using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_MemoryMapper : IItemAsyncMapper<Scale_MemoryBusiness, Scale_MemoryBusiness>
{
	public static Scale_MemoryMapper Instance { get; private set; } = new Scale_MemoryMapper();


	public Task<Scale_MemoryBusiness> MapAsync(string contextGuid, Scale_MemoryBusiness source, Scale_MemoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_MemoryBusiness(contextGuid);
			}
			destination.ScaleGuid = source.ScaleGuid;
			destination.MyProperty = source.MyProperty;
			destination.ProductGuid = source.ProductGuid;
			destination.MemoryNumber = source.MemoryNumber;
			destination.MemoryName = source.MemoryName;
			destination.MemoryPrice = source.MemoryPrice;
			destination.Date = source.Date;
			destination.Taeed = source.Taeed;
			destination.Vazni = source.Vazni;
			destination.TotalQT = source.TotalQT;
			destination.TotalPrice = source.TotalPrice;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_MemoryMapper.cs", 41);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_MemoryBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_MemoryBusiness> source, List<Scale_MemoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
