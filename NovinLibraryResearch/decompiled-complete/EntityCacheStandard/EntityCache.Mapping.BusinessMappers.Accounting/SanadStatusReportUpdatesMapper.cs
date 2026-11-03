using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class SanadStatusReportUpdatesMapper : IItemAsyncMapper<SanadStatusReportUpdatesBusiness, WebSanadStatusReportUpdates>, IItemAsyncMapper<SanadStatusReportUpdatesBusiness, SanadStatusReportUpdatesBusiness>, IItemAsyncMapper<WebSanadStatusReportUpdates, SanadStatusReportUpdatesBusiness>
{
	public static SanadStatusReportUpdatesMapper Instance { get; private set; } = new SanadStatusReportUpdatesMapper();


	public Task<WebSanadStatusReportUpdates> MapAsync(string contextGuid, SanadStatusReportUpdatesBusiness source, WebSanadStatusReportUpdates destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSanadStatusReportUpdates();
			}
			destination.Guid = source.Guid;
			destination.AsGuid = source.AsGuid;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.StatusId = source.StatusId;
			destination.CashierGuid = source.CashierGuid;
			destination.Ordinal = source.Ordinal;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusReportUpdatesMapper.cs", 48);
			throw;
		}
	}

	public Task<SanadStatusReportUpdatesBusiness> MapAsync(string contextGuid, SanadStatusReportUpdatesBusiness source, SanadStatusReportUpdatesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadStatusReportUpdatesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.AsGuid = source.AsGuid;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.StatusId = source.StatusId;
			destination.CashierGuid = source.CashierGuid;
			destination.Ordinal = source.Ordinal;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusReportUpdatesMapper.cs", 83);
			throw;
		}
	}

	public Task<SanadStatusReportUpdatesBusiness> MapAsync(string contextGuid, WebSanadStatusReportUpdates source, SanadStatusReportUpdatesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadStatusReportUpdatesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.AsGuid = source.AsGuid;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.StatusId = source.StatusId;
			destination.CashierGuid = source.CashierGuid;
			destination.Ordinal = source.Ordinal;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusReportUpdatesMapper.cs", 118);
			throw;
		}
	}

	public Task<List<WebSanadStatusReportUpdates>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportUpdatesBusiness> source, List<WebSanadStatusReportUpdates> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadStatusReportUpdatesBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportUpdatesBusiness> source, List<SanadStatusReportUpdatesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadStatusReportUpdatesBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanadStatusReportUpdates> source, List<SanadStatusReportUpdatesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
