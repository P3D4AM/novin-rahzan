using System;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Scale.EventArguments;

namespace PacketParser.Scale;

public interface IScaleConnection : IConnection, IDisposable
{
	ConnectionStatus Status { get; }

	ConnectionType ConnectionType { get; }

	event Func<object, ReadEventArgs, Task> Received;

	event Func<object, DisconnectEventArgs, Task> Disconnected;
}
