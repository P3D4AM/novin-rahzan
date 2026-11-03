using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_DateSH_HMapper : IItemAsyncMapper<Scale_DateSH_HBusiness, Scale_DateSH_HBusiness>
{
	public static Scale_DateSH_HMapper Instance { get; private set; } = new Scale_DateSH_HMapper();


	public Task<Scale_DateSH_HBusiness> MapAsync(string contextGuid, Scale_DateSH_HBusiness source, Scale_DateSH_HBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_DateSH_HBusiness(contextGuid);
			}
			destination.FactorNumber = source.FactorNumber;
			destination.UpdateDate = source.UpdateDate;
			destination.BatchGuid = source.BatchGuid;
			destination.ScaleGuid = source.ScaleGuid;
			destination.Guid = source.Guid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_DateSH_HMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_DateSH_HBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_DateSH_HBusiness> source, List<Scale_DateSH_HBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
