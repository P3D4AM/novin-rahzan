using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_Message_ReadSaleMapper : IItemAsyncMapper<Scale_Message_ReadSaleBusiness, Scale_Message_ReadSaleBusiness>
{
	public static Scale_Message_ReadSaleMapper Instance { get; private set; } = new Scale_Message_ReadSaleMapper();


	public Task<Scale_Message_ReadSaleBusiness> MapAsync(string contextGuid, Scale_Message_ReadSaleBusiness source, Scale_Message_ReadSaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_Message_ReadSaleBusiness(contextGuid);
			}
			destination.ScaleGuid = source.ScaleGuid;
			destination.MemoryGuid = source.MemoryGuid;
			destination.Count = source.Count;
			destination.DateM = source.DateM;
			destination.Priority = source.Priority;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_Message_ReadSaleMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_Message_ReadSaleBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Message_ReadSaleBusiness> source, List<Scale_Message_ReadSaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
