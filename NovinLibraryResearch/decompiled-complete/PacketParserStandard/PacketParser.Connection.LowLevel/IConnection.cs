using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Connection.LowLevel;

public interface IConnection : IDisposable
{
	string ConnectionId { get; set; }

	string ServerUniqueId { get; }

	CancellationTokenSource token { get; }

	EnConnectionType Type { get; }

	bool IsDisposed { get; }

	ENSource Source { get; }

	event Func<object, SingleConnectionSystemMessageEventArgs, Task> RecieveSystemMessages;

	event Func<object, SingleConnectionDataEventArgs, Task> Received;

	event Func<object, string, Task> StopConnection;

	event Func<object, System.EventArgs, Task> Reset;

	event Func<object, StateChange, Task> StateChanged;

	event Action Closed;

	void Stop(string Reason);

	Task<ReturnedSaveFuncInfo> Send(byte[] data, short TryCount = 0);

	Task<ReturnedSaveFuncInfo> Send(EnServerMessageInstruction instruction, string data, short TryCount = 0);

	Task Start(CancellationToken cancellationToken);

	Task<EnConnectionState> GetStateAsync();

	void ResetToken();
}
