using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Crm.Ticket;

public class TicketPartMapper : IItemAsyncMapper<TicketPartBusiness, WebTicketPart>, IItemAsyncMapper<TicketPartBusiness, TicketPartBusiness>, IItemAsyncMapper<WebTicketPart, TicketPartBusiness>
{
	public static TicketPartMapper Instance { get; private set; } = new TicketPartMapper();


	public Task<WebTicketPart> MapAsync(string contextGuid, TicketPartBusiness source, WebTicketPart destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTicketPart();
			}
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.ParentGuid = source.ParentGuid;
			destination.Description = source.Description;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketPartMapper.cs", 37);
			throw;
		}
	}

	public Task<List<WebTicketPart>> MapListAsync(string contextGuid, IEnumerable<TicketPartBusiness> source, List<WebTicketPart> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TicketPartBusiness> MapAsync(string contextGuid, TicketPartBusiness source, TicketPartBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketPartBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.ParentGuid = source.ParentGuid;
			destination.Description = source.Description;
			destination.Ordinal = source.Ordinal;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketPartMapper.cs", 65);
			throw;
		}
	}

	public Task<List<TicketPartBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketPartBusiness> source, List<TicketPartBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TicketPartBusiness> MapAsync(string contextGuid, WebTicketPart source, TicketPartBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketPartBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.ParentGuid = source.ParentGuid;
			destination.Description = source.Description;
			destination.Ordinal = source.Ordinal;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketPartMapper.cs", 93);
			throw;
		}
	}

	public Task<List<TicketPartBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTicketPart> source, List<TicketPartBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
