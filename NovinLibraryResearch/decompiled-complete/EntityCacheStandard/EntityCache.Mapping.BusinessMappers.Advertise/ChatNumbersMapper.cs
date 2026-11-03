using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class ChatNumbersMapper : IItemAsyncMapper<ChatNumberBusiness, ChatNumberBusiness>
{
	public static ChatNumbersMapper Instance { get; private set; } = new ChatNumbersMapper();


	public Task<ChatNumberBusiness> MapAsync(string contextGuid, ChatNumberBusiness source, ChatNumberBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChatNumberBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Number = source.Number;
			destination.DateM = source.DateM;
			destination.City = source.City;
			destination.Category = source.Category;
			destination.IsUsed = source.IsUsed;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\ChatNumbersMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ChatNumberBusiness>> MapListAsync(string contextGuid, IEnumerable<ChatNumberBusiness> source, List<ChatNumberBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
