using System.Collections.Concurrent;
using System.Threading;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class ProductOnBusinessSavedEventArgs
{
	public WebProduct Product { get; set; }

	public EnSystem System { get; set; }

	public ConcurrentDictionary<string, string> Changes { get; set; }

	public CancellationToken Token { get; set; }
}
