using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace SignalRCoreConnectionCore.Mappers;

public class ConnectionStateMapper : IItemAsyncMapper<MapperItemContainer<HubConnectionState>, MapperItemContainer<EnConnectionState>>
{
	private static Lazy<ConnectionStateMapper> instance = new Lazy<ConnectionStateMapper>();

	public static ConnectionStateMapper Instance => instance.Value;

	public Task<MapperItemContainer<EnConnectionState>> MapAsync(string contextGuid, MapperItemContainer<HubConnectionState> source, MapperItemContainer<EnConnectionState> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			destination = new MapperItemContainer<EnConnectionState>();
		}
		try
		{
			switch (source.Value)
			{
			case HubConnectionState.Disconnected:
				destination.Value = EnConnectionState.Disconnected;
				break;
			case HubConnectionState.Connected:
				destination.Value = EnConnectionState.Connected;
				break;
			case HubConnectionState.Connecting:
				destination.Value = EnConnectionState.Connecting;
				break;
			case HubConnectionState.Reconnecting:
				destination.Value = EnConnectionState.Reconnecting;
				break;
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\Mappers\\ConnectionStateMapper.cs", 40);
		}
		return null;
	}

	public Task<List<MapperItemContainer<EnConnectionState>>> MapListAsync(string contextGuid, IEnumerable<MapperItemContainer<HubConnectionState>> source, List<MapperItemContainer<EnConnectionState>> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
