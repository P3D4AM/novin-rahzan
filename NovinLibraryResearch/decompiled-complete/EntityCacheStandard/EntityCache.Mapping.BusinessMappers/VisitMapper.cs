using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class VisitMapper : IItemAsyncMapper<VisitBusiness, WebVisit>, IItemAsyncMapper<VisitBusiness, VisitBusiness>, IItemAsyncMapper<WebVisit, VisitBusiness>
{
	public static VisitMapper Instance { get; private set; } = new VisitMapper();


	public Task<WebVisit> MapAsync(string contextGuid, VisitBusiness source, WebVisit destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebVisit();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ScheduledDate = source.ScheduledDate;
			destination.StartDate = source.StartDate;
			destination.CreationdDate = source.CreationdDate;
			destination.Description = source.Description;
			destination.VisitNo = source.VisitNo;
			destination.VisitIsCompleted = source.VisitIsCompleted;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.TicketGuids = source.TicketGuids?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitMapper.cs", 75);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebVisit>> MapListAsync(string contextGuid, IEnumerable<VisitBusiness> source, List<WebVisit> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<VisitBusiness> MapAsync(string contextGuid, VisitBusiness source, VisitBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			num = 1;
			if (destination == null)
			{
				destination = new VisitBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			num = 2;
			destination.Guid = source.Guid;
			num = 3;
			destination.Modified = source.Modified;
			num = 4;
			destination.ScheduledDate = source.ScheduledDate;
			num = 5;
			destination.StartDate = source.StartDate;
			num = 6;
			destination.CreationdDate = source.CreationdDate;
			num = 7;
			destination.Description = source.Description;
			num = 8;
			destination.VisitNo = source.VisitNo;
			num = 9;
			destination.VisitIsCompleted = source.VisitIsCompleted;
			num = 10;
			destination.ServerStatus = source.ServerStatus;
			num = 11;
			destination.DeliveryDate = source.DeliveryDate;
			num = 12;
			destination.DeliveryId = source.DeliveryId;
			num = 13;
			destination.DeliveryMessage = source.DeliveryMessage;
			num = 14;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			num = 15;
			destination.HostRefrenced = source.HostRefrenced;
			num = 16;
			destination.TicketGuids = source?.TicketGuids?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Line Error Is {num} ", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitMapper.cs", 143);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<VisitBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitBusiness> source, List<VisitBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<VisitBusiness> MapAsync(string contextGuid, WebVisit source, VisitBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			num = 1;
			if (destination == null)
			{
				destination = new VisitBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			num = 2;
			destination.Guid = source.Guid;
			num = 3;
			destination.Modified = source.Modified;
			num = 4;
			destination.ScheduledDate = source.ScheduledDate;
			num = 5;
			destination.StartDate = source.StartDate;
			num = 6;
			destination.CreationdDate = source.CreationdDate;
			num = 7;
			destination.Description = source.Description;
			num = 8;
			destination.VisitNo = source.VisitNo;
			num = 9;
			destination.VisitIsCompleted = source.VisitIsCompleted;
			num = 10;
			destination.ServerStatus = source.ServerStatus;
			num = 11;
			destination.DeliveryDate = source.DeliveryDate;
			num = 12;
			destination.DeliveryId = source.DeliveryId;
			num = 13;
			destination.DeliveryMessage = source.DeliveryMessage;
			num = 14;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			num = 15;
			destination.HostRefrenced = source.HostRefrenced;
			num = 16;
			destination.TicketGuids = source.TicketGuids?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $" The Error In Line :{num}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\VisitMapper.cs", 211);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<VisitBusiness>> MapListAsync(string contextGuid, IEnumerable<WebVisit> source, List<VisitBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
