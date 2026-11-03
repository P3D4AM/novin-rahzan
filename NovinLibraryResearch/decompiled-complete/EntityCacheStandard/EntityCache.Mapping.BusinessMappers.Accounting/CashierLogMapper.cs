using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class CashierLogMapper : IItemAsyncMapper<CashiersLogBusiness, WebCashierLog>, IItemAsyncMapper<CashiersLogBusiness, CashiersLogBusiness>, IItemAsyncMapper<WebCashierLog, CashiersLogBusiness>
{
	public static CashierLogMapper Instance { get; private set; } = new CashierLogMapper();


	public Task<WebCashierLog> MapAsync(string contextGuid, CashiersLogBusiness source, WebCashierLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCashierLog();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.NewSerializedData = source.NewSerializedData;
			destination.OldSerializedData = source.OldSerializedData;
			destination.Action = source.Action;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.System = source.System;
			destination.DateUtc = source.DateUtc;
			destination.Source = source.Source;
			destination.ApplicationVersion = source.ApplicationVersion;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierLogMapper.cs", 50);
			throw;
		}
	}

	public Task<CashiersLogBusiness> MapAsync(string contextGuid, CashiersLogBusiness source, CashiersLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CashiersLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.NewSerializedData = source.NewSerializedData;
			destination.OldSerializedData = source.OldSerializedData;
			destination.Action = source.Action;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.System = source.System;
			destination.DateUtc = source.DateUtc;
			destination.Source = source.Source;
			destination.ApplicationVersion = source.ApplicationVersion;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierLogMapper.cs", 89);
			throw;
		}
	}

	public Task<CashiersLogBusiness> MapAsync(string contextGuid, WebCashierLog source, CashiersLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CashiersLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.NewSerializedData = source.NewSerializedData;
			destination.OldSerializedData = source.OldSerializedData;
			destination.Action = source.Action;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.System = source.System;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.Source = source.Source;
			destination.ApplicationVersion = source.ApplicationVersion;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CashierLogMapper.cs", 128);
			throw;
		}
	}

	public Task<List<CashiersLogBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCashierLog> source, List<CashiersLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CashiersLogBusiness>> MapListAsync(string contextGuid, IEnumerable<CashiersLogBusiness> source, List<CashiersLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCashierLog>> MapListAsync(string contextGuid, IEnumerable<CashiersLogBusiness> source, List<WebCashierLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
