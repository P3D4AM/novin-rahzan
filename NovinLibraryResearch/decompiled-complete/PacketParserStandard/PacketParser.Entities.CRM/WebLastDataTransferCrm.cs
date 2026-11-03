using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities.CRM;

public class WebLastDataTransferCrm : Serializable<WebLastDataTransferCrm>, ILastDataTransferCrm
{
	public long hashId { get; set; }

	public int DepUserId { get; set; }

	public int DepId { get; set; }

	public int DisconnectedCount { get; set; }

	public long SumRecieved { get; set; }

	public long SumSent { get; set; }

	public DateTime LastConnectionTime { get; set; }

	public string Username { get; set; }

	public ENSource Source { get; set; }

	public DateTime LastSync { get; set; }

	public bool IsOnline { get; set; }
}
