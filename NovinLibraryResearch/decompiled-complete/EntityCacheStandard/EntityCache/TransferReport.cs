using System;
using PacketParser.Entities;

namespace EntityCache;

public class TransferReport
{
	public string Description { get; set; }

	public string GroupName { get; set; }

	public DateTime Date { get; set; }

	public string Time => Date.ToString("T");

	public ReturnedState State { get; set; }

	public byte[] Image { get; set; }
}
