using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_Message_WriteMemoryMapper : IItemAsyncMapper<Scale_Message_WriteMemoryBusiness, Scale_Message_WriteMemoryBusiness>
{
	public static Scale_Message_WriteMemoryMapper Instance { get; private set; }

	public Task<Scale_Message_WriteMemoryBusiness> MapAsync(string contextGuid, Scale_Message_WriteMemoryBusiness source, Scale_Message_WriteMemoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_Message_WriteMemoryBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.ScaleGuid = source.ScaleGuid;
			destination.Code = source.Code;
			destination.Barcode = source.Barcode;
			destination.Name = source.Name;
			destination.UnitPrice = source.UnitPrice;
			destination.DateM = source.DateM;
			destination.Expiredate = source.Expiredate;
			destination.ST = source.ST;
			destination.Vazni = source.Vazni;
			destination.NextTry = source.NextTry;
			destination.TryCount = source.TryCount;
			destination.MaxTryCount = source.MaxTryCount;
			destination.Parametr1 = source.Parametr1;
			destination.Parametr2 = source.Parametr2;
			destination.Parametr3 = source.Parametr3;
			destination.Parametr4 = source.Parametr4;
			destination.MemoryNumber = source.MemoryNumber;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_Message_WriteMemoryMapper.cs", 47);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_Message_WriteMemoryBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Message_WriteMemoryBusiness> source, List<Scale_Message_WriteMemoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
