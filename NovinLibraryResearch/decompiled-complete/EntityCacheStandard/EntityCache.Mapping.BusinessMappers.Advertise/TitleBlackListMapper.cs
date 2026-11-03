using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class TitleBlackListMapper : IItemAsyncMapper<TitleBlackListBusiness, TitleBlackListBusiness>
{
	public static TitleBlackListMapper Instance { get; private set; } = new TitleBlackListMapper();


	public Task<TitleBlackListBusiness> MapAsync(string contextGuid, TitleBlackListBusiness source, TitleBlackListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TitleBlackListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Words = source.Words;
			destination.Is_Checked = source.Is_Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\TitleBlackListMapper.cs", 33);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<TitleBlackListBusiness>> MapListAsync(string contextGuid, IEnumerable<TitleBlackListBusiness> source, List<TitleBlackListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
