using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class ContentBlackListMapper : IItemAsyncMapper<ContentBlackListBusiness, ContentBlackListBusiness>
{
	public static ContentBlackListMapper Instance { get; private set; } = new ContentBlackListMapper();


	public Task<ContentBlackListBusiness> MapAsync(string contextGuid, ContentBlackListBusiness source, ContentBlackListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ContentBlackListBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Words = source.Words;
			destination.Is_Checked = source.Is_Checked;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\ContentBlackListMapper.cs", 33);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ContentBlackListBusiness>> MapListAsync(string contextGuid, IEnumerable<ContentBlackListBusiness> source, List<ContentBlackListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
