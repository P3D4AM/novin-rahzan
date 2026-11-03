using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_MemoryHistoryMapper : IItemAsyncMapper<Scale_MemoryHistoryBusiness, Scale_MemoryHistoryBusiness>
{
	public static Scale_MemoryHistoryMapper Instance { get; private set; } = new Scale_MemoryHistoryMapper();


	public Task<Scale_MemoryHistoryBusiness> MapAsync(string contextGuid, Scale_MemoryHistoryBusiness source, Scale_MemoryHistoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_MemoryHistoryBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.ScaleGuid = source.ScaleGuid;
			destination.Date = source.Date;
			destination.ScaleName = source.ScaleName;
			destination.ProductCode = source.ProductCode;
			destination.BarCode = source.BarCode;
			destination.ProductName = source.ProductName;
			destination.MemoryNumber = source.MemoryNumber;
			destination.Vazni = source.Vazni;
			destination.UnitPrice = source.UnitPrice;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_MemoryHistoryMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_MemoryHistoryBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_MemoryHistoryBusiness> source, List<Scale_MemoryHistoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
