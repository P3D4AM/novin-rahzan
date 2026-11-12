using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Connections;

namespace Microsoft.AspNetCore.SignalR.Protocol;

public interface IHubProtocol
{
	string Name { get; }

	int Version { get; }

	TransferFormat TransferFormat { get; }

	bool TryParseMessage(ref ReadOnlySequence<byte> input, IInvocationBinder binder, [NotNullWhen(true)] out HubMessage? message);

	void WriteMessage(HubMessage message, IBufferWriter<byte> output);

	ReadOnlyMemory<byte> GetMessageBytes(HubMessage message);

	bool IsVersionSupported(int version);
}
