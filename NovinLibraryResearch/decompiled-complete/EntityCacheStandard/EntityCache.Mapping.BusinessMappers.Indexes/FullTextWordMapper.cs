using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Indexes;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Indexes;

internal class FullTextWordMapper : IItemAsyncMapper<FullTextWordBusiness, FullTextWordBusiness>
{
	public static FullTextWordMapper Instance { get; private set; } = new FullTextWordMapper();


	public Task<FullTextWordBusiness> MapAsync(string contextGuid, FullTextWordBusiness source, FullTextWordBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FullTextWordBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Id = source.Id;
			destination.Word = source.Word;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Indexes\\FullTextWordMapper.cs", 32);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<FullTextWordBusiness>> MapListAsync(string contextGuid, IEnumerable<FullTextWordBusiness> source, List<FullTextWordBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
