using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class AdvTokensMapper : IItemAsyncMapper<AdvTokensBusiness, AdvTokensBusiness>
{
	public static AdvTokensMapper Instance { get; private set; } = new AdvTokensMapper();


	public Task<AdvTokensBusiness> MapAsync(string contextGuid, AdvTokensBusiness source, AdvTokensBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AdvTokensBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Number = source.Number;
			destination.Type = source.Type;
			destination.Token = source.Token;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\AdvTokensMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<AdvTokensBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvTokensBusiness> source, List<AdvTokensBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
