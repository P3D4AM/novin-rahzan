using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Notification;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class CrmNotificationMapper : IItemAsyncMapper<CrmNotificationBusiness, WebCrmNotification>, IItemAsyncMapper<CrmNotificationBusiness, CrmNotificationBusiness>, IItemAsyncMapper<WebCrmNotification, CrmNotificationBusiness>
{
	public static CrmNotificationMapper Instance { get; private set; } = new CrmNotificationMapper();


	public Task<WebCrmNotification> MapAsync(string contextGuid, CrmNotificationBusiness source, WebCrmNotification destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCrmNotification();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Type = source.Type;
			destination.EventTime = source.EventTime;
			destination.Uuid = source.Uuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CrmNotificationMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<CrmNotificationBusiness> MapAsync(string contextGuid, CrmNotificationBusiness source, CrmNotificationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CrmNotificationBusiness();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Duration = source.Duration;
			destination.Type = source.Type;
			destination.EventTime = source.EventTime;
			destination.DialedNumber = source.DialedNumber;
			destination.IsIncoming = source.IsIncoming;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.CoGuid = source.CoGuid;
			destination.Ordinal = source.Ordinal;
			destination.Ring = source.Ring;
			destination.ContextGuid = contextGuid;
			destination.Uuid = source.Uuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CrmNotificationMapper.cs", 69);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<CrmNotificationBusiness> MapAsync(string contextGuid, WebCrmNotification source, CrmNotificationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CrmNotificationBusiness();
			}
			destination.Guid = source.Guid;
			destination.Type = source.Type;
			destination.EventTime = source.EventTime;
			destination.ContextGuid = contextGuid;
			destination.Uuid = source.Uuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\CrmNotificationMapper.cs", 93);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CrmNotificationBusiness>> MapListAsync(string contextGuid, IEnumerable<CrmNotificationBusiness> source, List<CrmNotificationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CrmNotificationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCrmNotification> source, List<CrmNotificationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCrmNotification>> MapListAsync(string contextGuid, IEnumerable<CrmNotificationBusiness> source, List<WebCrmNotification> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
