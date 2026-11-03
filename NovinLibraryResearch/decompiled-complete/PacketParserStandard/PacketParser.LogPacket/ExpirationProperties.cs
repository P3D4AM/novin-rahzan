using System;
using System.Threading.Tasks;

namespace PacketParser.LogPacket;

public class ExpirationProperties
{
	internal Action<PacketSignature> Action { get; set; }

	internal Task Timeout { get; set; }

	public bool IsProcessed { get; set; }
}
