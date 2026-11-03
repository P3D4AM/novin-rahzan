using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

public class RoomKardexMapper : IItemAsyncMapper<RoomKardexBusiness, RoomKardexBusiness>
{
	public static RoomKardexMapper Instance { get; private set; }

	public Task<RoomKardexBusiness> MapAsync(string contextGuid, RoomKardexBusiness source, RoomKardexBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RoomKardexBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = destination.Guid;
			destination.RoomGuid = destination.RoomGuid;
			destination.AsGuid = source.AsGuid;
			destination.CashiarGuid = source.CashiarGuid;
			destination.OldStatus = source.OldStatus;
			destination.NewStatus = source.NewStatus;
			destination.DateM = source.DateM;
			destination.Ordinal = source.Ordinal;
			destination.Discription = source.Discription;
			destination.AsType = source.AsType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\RoomKardexMapper.cs", 42);
			throw;
		}
	}

	public Task<List<RoomKardexBusiness>> MapListAsync(string contextGuid, IEnumerable<RoomKardexBusiness> source, List<RoomKardexBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
