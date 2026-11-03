using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class StimulQueryMapper : IItemAsyncMapper<StimulQueryBusiness, StimulQueryBusiness>
{
	public static StimulQueryMapper Instance { get; private set; } = new StimulQueryMapper();


	public Task<StimulQueryBusiness> MapAsync(string contextGuid, StimulQueryBusiness source, StimulQueryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new StimulQueryBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.RefrencedId = source.RefrencedId;
			destination.Order = source.Order;
			destination.Date = source.Date;
			destination.Id = source.Id;
			destination.SecondKey = source.SecondKey;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\StimulQueryMapper.cs", 37);
			throw;
		}
	}

	public Task<List<StimulQueryBusiness>> MapListAsync(string contextGuid, IEnumerable<StimulQueryBusiness> source, List<StimulQueryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
