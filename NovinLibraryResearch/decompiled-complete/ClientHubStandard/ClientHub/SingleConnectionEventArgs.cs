using System;
using System.Threading;

namespace ClientHub;

public class SingleConnectionEventArgs : EventArgs
{
	public string ConnectionUrl { get; set; }

	public CancellationToken cancellationToken { get; set; }

	public string ConnectionId { get; set; }
}
