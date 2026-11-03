using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Scale;

namespace ClientHub.Mapper;

public class TcpConnectionStatusMapper : IItemAsyncMapper<MapperItemContainer<ConnectionStatus>, MapperItemContainer<EnConnectionState>>
{
	public static TcpConnectionStatusMapper Instance { get; private set; } = new TcpConnectionStatusMapper();


	public Task<MapperItemContainer<EnConnectionState>> MapAsync(string contextGuid, MapperItemContainer<ConnectionStatus> source, MapperItemContainer<EnConnectionState> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			destination = new MapperItemContainer<EnConnectionState>();
		}
		try
		{
			switch (source.Value)
			{
			case ConnectionStatus.Disconnected:
				destination.Value = EnConnectionState.Disconnected;
				break;
			case ConnectionStatus.Connected:
				destination.Value = EnConnectionState.Connected;
				break;
			case ConnectionStatus.Connecting:
				destination.Value = EnConnectionState.Connecting;
				break;
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Mapper\\TcpConnectionStatusMapper.cs", 29);
		}
		return null;
	}

	public Task<List<MapperItemContainer<EnConnectionState>>> MapListAsync(string contextGuid, IEnumerable<MapperItemContainer<ConnectionStatus>> source, List<MapperItemContainer<EnConnectionState>> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
