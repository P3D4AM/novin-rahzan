using System.Threading;
using PacketParser.Entities;

namespace PacketParser.Connection.LowLevel;

public class SingleConnectionSystemMessageEventArgs
{
	public CancellationToken cancellationToken { get; set; }

	public string ConnectionId { get; set; }

	public string Url { get; set; }

	public string Data { get; set; }

	public EnServerMessageInstruction Instruction { get; set; }
}
