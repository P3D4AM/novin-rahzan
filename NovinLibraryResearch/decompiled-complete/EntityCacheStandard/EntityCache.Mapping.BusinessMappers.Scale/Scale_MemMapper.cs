using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_MemMapper : IItemAsyncMapper<Scale_MemBusiness, Scale_MemBusiness>
{
	public static Scale_MemMapper Instance { get; private set; } = new Scale_MemMapper();


	public Task<Scale_MemBusiness> MapAsync(string contextGuid, Scale_MemBusiness source, Scale_MemBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_MemBusiness(contextGuid);
			}
			destination.ScaleGuid = source.ScaleGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.MemoryNumber = source.MemoryNumber;
			destination.Date = source.Date;
			destination.ISSaved = source.ISSaved;
			destination.PriceOnScale = source.PriceOnScale;
			destination.CodeOnScale = source.CodeOnScale;
			destination.NameScale = source.NameScale;
			destination.BarCode = source.BarCode;
			destination.Name = source.Name;
			destination.Price = source.Price;
			destination.Vazni = source.Vazni;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_MemMapper.cs", 42);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_MemBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_MemBusiness> source, List<Scale_MemBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
