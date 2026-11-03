using System;

namespace PacketParser.Activation;

public class LKFailed
{
	public long LKSerial { get; set; }

	public DateTime DateEX { get; set; } = DateTime.Now;


	public string Description { get; set; } = "";


	public short Status { get; set; }
}
