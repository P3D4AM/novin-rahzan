using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Entities.Global;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class PushNotificationMapper : IItemAsyncMapper<WebPushNotification, PushNotificationBusiness>, IItemAsyncMapper<PushNotificationBusiness, PushNotificationBusiness>
{
	public static PushNotificationMapper Instance { get; private set; } = new PushNotificationMapper();


	public Task<PushNotificationBusiness> MapAsync(string contextGuid, WebPushNotification source, PushNotificationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new PushNotificationBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Source = source.Source;
			destination.Type = source.Type;
			destination.Date = source.Date;
			destination.ExpireDate = source.ExpireDate;
			destination.Message = source.Message;
			destination.Title = source.Title;
			destination.DisplayCount = source.DisplayCount;
			destination.ModifiedDate = source.ModifiedDate;
			destination.HelperUri = source.HelperUri;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\PushNotoficationMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<PushNotificationBusiness> MapAsync(string contextGuid, PushNotificationBusiness source, PushNotificationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new PushNotificationBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Source = source.Source;
			destination.Type = source.Type;
			destination.Date = source.Date;
			destination.ExpireDate = source.ExpireDate;
			destination.Message = source.Message;
			destination.Title = source.Title;
			destination.DisplayCount = source.DisplayCount;
			destination.ModifiedDate = source.ModifiedDate;
			destination.HelperUri = source.HelperUri;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\PushNotoficationMapper.cs", 64);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PushNotificationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPushNotification> source, List<PushNotificationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PushNotificationBusiness>> MapListAsync(string contextGuid, IEnumerable<PushNotificationBusiness> source, List<PushNotificationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
