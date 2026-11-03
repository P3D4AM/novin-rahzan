using System;

namespace PacketParser.Entities.Interfaces;

public interface ILastDataTransferCrm
{
	long hashId { get; set; }

	int DepUserId { get; set; }

	string Username { get; set; }

	int DisconnectedCount { get; set; }

	long SumRecieved { get; set; }

	long SumSent { get; set; }

	DateTime LastConnectionTime { get; set; }

	ENSource Source { get; set; }

	DateTime LastSync { get; set; }

	bool IsOnline { get; set; }
}
