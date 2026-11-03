using System;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;

namespace ClientHub;

public interface ISignalRConnectionFactory<T> : IDisposable, IHasContext
{
	event EventHandler Connected;

	event EventHandler Disconnected;

	RetriableHubConnection Get(T type);

	Task ConnectAsync();

	void Disconnect();
}
