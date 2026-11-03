using System.Threading;

namespace PacketParser.Scale.EventArguments;

public class MultipleConnectionStatusEventArgs
{
	public MultipleConnectionStatus Status { get; set; }

	public CancellationToken Token { get; set; }
}
