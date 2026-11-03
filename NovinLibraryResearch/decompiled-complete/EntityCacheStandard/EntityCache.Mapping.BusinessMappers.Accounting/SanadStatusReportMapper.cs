using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Report;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class SanadStatusReportMapper : IItemAsyncMapper<SanadStatusReportBusiness, WebSanadStatusReport>, IItemAsyncMapper<SanadStatusReportBusiness, SanadStatusReportBusiness>, IItemAsyncMapper<WebSanadStatusReport, SanadStatusReportBusiness>
{
	public static SanadStatusReportMapper Instance { get; private set; } = new SanadStatusReportMapper();


	public Task<WebSanadStatusReport> MapAsync(string contextGuid, SanadStatusReportBusiness source, WebSanadStatusReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSanadStatusReport();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id = source.Id;
			destination.AsType = source.AsType;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.MetaData = source.MetaData;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ErrorMinutes = source.ErrorMinutes;
			destination.WarningMinutes = source.WarningMinutes;
			destination.Id2From = source.Id2From;
			destination.Id2To = source.Id2To;
			destination.InactivityDays = source.InactivityDays;
			destination.TargetStatusId = source.TargetStatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusReportMapper.cs", 50);
			throw;
		}
	}

	public Task<SanadStatusReportBusiness> MapAsync(string contextGuid, SanadStatusReportBusiness source, SanadStatusReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadStatusReportBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id = source.Id;
			destination.AsType = source.AsType;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.MetaData = source.MetaData;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ErrorMinutes = source.ErrorMinutes;
			destination.WarningMinutes = source.WarningMinutes;
			destination.Id2From = source.Id2From;
			destination.Id2To = source.Id2To;
			destination.InactivityDays = source.InactivityDays;
			destination.TargetStatusId = source.TargetStatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusReportMapper.cs", 89);
			throw;
		}
	}

	public Task<SanadStatusReportBusiness> MapAsync(string contextGuid, WebSanadStatusReport source, SanadStatusReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadStatusReportBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id = source.Id;
			destination.AsType = source.AsType;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.MetaData = source.MetaData;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ErrorMinutes = source.ErrorMinutes;
			destination.WarningMinutes = source.WarningMinutes;
			destination.Id2From = source.Id2From;
			destination.Id2To = source.Id2To;
			destination.InactivityDays = source.InactivityDays;
			destination.TargetStatusId = source.TargetStatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusReportMapper.cs", 128);
			throw;
		}
	}

	public Task<List<WebSanadStatusReport>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportBusiness> source, List<WebSanadStatusReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadStatusReportBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadStatusReportBusiness> source, List<SanadStatusReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadStatusReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanadStatusReport> source, List<SanadStatusReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
