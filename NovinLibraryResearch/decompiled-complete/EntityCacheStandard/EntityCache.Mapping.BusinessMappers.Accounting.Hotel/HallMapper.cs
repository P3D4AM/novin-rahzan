using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

public class HallMapper : IItemAsyncMapper<HallBusiness, HallBusiness>
{
	public static HallMapper Instance { get; private set; } = new HallMapper();


	public Task<HallBusiness> MapAsync(string contextGuid, HallBusiness source, HallBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HallBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Title = source.Title;
			destination.MasterGuid = source.MasterGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Bill\\HallMapper.cs", 35);
			throw;
		}
	}

	public Task<List<HallBusiness>> MapListAsync(string contextGuid, IEnumerable<HallBusiness> source, List<HallBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
