using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

internal static class WebSocketExtensions
{
	public static ValueTask SendAsync(this WebSocket webSocket, ReadOnlySequence<byte> buffer, WebSocketMessageType webSocketMessageType, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (buffer.IsSingleSegment)
		{
			MemoryMarshal.TryGetArray(buffer.First, out var segment);
			return new ValueTask(webSocket.SendAsync(segment, webSocketMessageType, endOfMessage: true, cancellationToken));
		}
		return SendMultiSegmentAsync(webSocket, buffer, webSocketMessageType, cancellationToken);
	}

	private static async ValueTask SendMultiSegmentAsync(WebSocket webSocket, ReadOnlySequence<byte> buffer, WebSocketMessageType webSocketMessageType, CancellationToken cancellationToken = default(CancellationToken))
	{
		SequencePosition position = buffer.Start;
		ReadOnlyMemory<byte> memory;
		while (buffer.TryGet(ref position, out memory))
		{
			MemoryMarshal.TryGetArray(memory, out var segment);
			await webSocket.SendAsync(segment, webSocketMessageType, endOfMessage: false, cancellationToken);
		}
		await webSocket.SendAsync(new ArraySegment<byte>(Array.Empty<byte>()), webSocketMessageType, endOfMessage: true, cancellationToken);
	}
}
