using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class SaveTempMapper : IItemAsyncMapper<SaveTempBusiness, SaveTempBusiness>
{
	public static SaveTempMapper Instance { get; private set; } = new SaveTempMapper();


	public Task<SaveTempBusiness> MapAsync(string contextGuid, SaveTempBusiness source, SaveTempBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SaveTempBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Id2 = source.Id2;
			destination.Description = source.Description;
			destination.SerialedData = source.SerialedData;
			destination.DateM = source.DateM;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\SaveTempMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SaveTempBusiness>> MapListAsync(string contextGuid, IEnumerable<SaveTempBusiness> source, List<SaveTempBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
