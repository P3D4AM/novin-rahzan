using System.IO;
using System.Threading;

namespace System.Net.WebSockets;

public static class WebSocketProtocol
{
	public static WebSocket CreateFromStream(Stream stream, bool isServer, string subProtocol, TimeSpan keepAliveInterval)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanRead || !stream.CanWrite)
		{
			throw new ArgumentException((!stream.CanRead) ? System.SR.NotReadableStream : System.SR.NotWriteableStream, "stream");
		}
		if (subProtocol != null)
		{
			System.Net.WebSockets.WebSocketValidate.ValidateSubprotocol(subProtocol);
		}
		if (keepAliveInterval != Timeout.InfiniteTimeSpan && keepAliveInterval < TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException("keepAliveInterval", keepAliveInterval, System.SR.Format(System.SR.net_WebSockets_ArgumentOutOfRange_TooSmall, 0));
		}
		return System.Net.WebSockets.ManagedWebSocket.CreateFromConnectedStream(stream, isServer, subProtocol, keepAliveInterval);
	}
}
