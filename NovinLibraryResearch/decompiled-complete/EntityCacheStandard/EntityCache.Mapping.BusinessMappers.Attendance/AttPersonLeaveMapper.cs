using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class AttPersonLeaveMapper : IItemAsyncMapper<AttPersonLeaveBusiness, AttPersonLeaveBusiness>, IItemAsyncMapper<WebAttPersonLeave, AttPersonLeaveBusiness>, IItemAsyncMapper<AttPersonLeaveBusiness, WebAttPersonLeave>, IItemAsyncMapper<WebAttPersonLeave, WebAttTempVacations>, IItemAsyncMapper<WebAttTempVacations, WebAttPersonLeave>
{
	public static AttPersonLeaveMapper Instance { get; private set; } = new AttPersonLeaveMapper();


	public Task<AttPersonLeaveBusiness> MapAsync(string contextGuid, AttPersonLeaveBusiness source, AttPersonLeaveBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AttPersonLeaveBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.PersonGuid = source.PersonGuid;
			destination.LeaveStatus = source.LeaveStatus;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.Reason = source.Reason;
			destination.Type = source.Type;
			destination.ManagerDate = source.ManagerDate;
			destination.ManagerMessage = source.ManagerMessage;
			destination.Modified = source.Modified;
			destination.Kind = source.Kind;
			destination.RequestDate = source.RequestDate;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Quiddity = source.Quiddity;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttPersonLeaveMapper.cs", 52);
			throw;
		}
	}

	public Task<WebAttPersonLeave> MapAsync(string contextGuid, AttPersonLeaveBusiness source, WebAttPersonLeave destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAttPersonLeave();
			}
			destination.Guid = source.Guid;
			destination.PersonGuid = source.PersonGuid;
			destination.Status = source.LeaveStatus;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.Reason = source.Reason;
			destination.Type = source.Type;
			destination.ManagerDate = source.ManagerDate;
			destination.ManagerMessage = source.ManagerMessage;
			destination.Kind = source.Kind;
			destination.RequestDate = source.RequestDate;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Quiddity = source.Quiddity;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttPersonLeaveMapper.cs", 88);
			throw;
		}
	}

	public Task<AttPersonLeaveBusiness> MapAsync(string contextGuid, WebAttPersonLeave source, AttPersonLeaveBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AttPersonLeaveBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.PersonGuid = source.PersonGuid;
			destination.LeaveStatus = source.Status;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.Reason = source.Reason;
			destination.Type = source.Type;
			destination.ManagerDate = source.ManagerDate;
			destination.ManagerMessage = source.ManagerMessage;
			destination.Kind = source.Kind;
			destination.RequestDate = source.RequestDate;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.FullName = source.PersonName;
			destination.Quiddity = source.Quiddity;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttPersonLeaveMapper.cs", 126);
			throw;
		}
	}

	public Task<WebAttPersonLeave> MapAsync(string contextGuid, WebAttTempVacations source, WebAttPersonLeave destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAttPersonLeave();
			}
			destination.Guid = source.Guid;
			destination.PersonGuid = source.PersonGuid;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.Reason = source.Reason;
			destination.Type = source.Type;
			destination.Kind = source.Kind;
			destination.PersonName = source.PersonName;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttPersonLeaveMapper.cs", 153);
			throw;
		}
	}

	public Task<WebAttTempVacations> MapAsync(string contextGuid, WebAttPersonLeave source, WebAttTempVacations destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAttTempVacations();
			}
			destination.Guid = source.Guid;
			destination.PersonGuid = source.PersonGuid;
			destination.DateFrom = source.DateFrom;
			destination.DateTo = source.DateTo;
			destination.Reason = source.Reason;
			destination.Type = source.Type;
			destination.Kind = source.Kind;
			destination.PersonName = source.PersonName;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttPersonLeaveMapper.cs", 180);
			throw;
		}
	}

	public Task<List<AttPersonLeaveBusiness>> MapListAsync(string contextGuid, IEnumerable<AttPersonLeaveBusiness> source, List<AttPersonLeaveBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebAttPersonLeave>> MapListAsync(string contextGuid, IEnumerable<AttPersonLeaveBusiness> source, List<WebAttPersonLeave> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AttPersonLeaveBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAttPersonLeave> source, List<AttPersonLeaveBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebAttPersonLeave>> MapListAsync(string contextGuid, IEnumerable<WebAttTempVacations> source, List<WebAttPersonLeave> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebAttTempVacations>> MapListAsync(string contextGuid, IEnumerable<WebAttPersonLeave> source, List<WebAttTempVacations> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
