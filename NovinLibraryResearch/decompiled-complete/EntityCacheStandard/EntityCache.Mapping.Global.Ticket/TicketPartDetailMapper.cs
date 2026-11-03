using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tickets;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global.Ticket;

public class TicketPartDetailMapper : IItemAsyncMapper<TicketPartDetailBusiness, TicketPartDetailBusiness>
{
	public static TicketPartDetailMapper Instance = new TicketPartDetailMapper();

	public Task<TicketPartDetailBusiness> MapAsync(string contextGuid, TicketPartDetailBusiness source, TicketPartDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketPartDetailBusiness(contextGuid);
			}
			destination.MasterGuid = source.MasterGuid;
			destination.TargetGuid = source.TargetGuid;
			destination.TargetType = source.TargetType;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketPartDetailMapper.cs", 34);
			throw;
		}
	}

	public Task<List<TicketPartDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketPartDetailBusiness> source, List<TicketPartDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
