using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class NaghzMapper : IItemAsyncMapper<NaghzBusiness, NaghzBusiness>
{
	public static NaghzMapper Instance { get; private set; } = new NaghzMapper();


	public Task<NaghzBusiness> MapAsync(string contextGuid, NaghzBusiness source, NaghzBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NaghzBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Message = source.Message;
			destination.Count = source.Count;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\NaghzMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<NaghzBusiness>> MapListAsync(string contextGuid, IEnumerable<NaghzBusiness> source, List<NaghzBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
