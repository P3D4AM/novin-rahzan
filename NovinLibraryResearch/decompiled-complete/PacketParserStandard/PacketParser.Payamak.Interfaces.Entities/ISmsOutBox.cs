using System;
using PacketParser.Entities;

namespace PacketParser.Payamak.Interfaces.Entities;

public interface ISmsOutBox : IBaseSms
{
	DateTime SchadulingM { get; set; }

	SMSSchadulingType SchadulingType { get; set; }

	short Priority { get; set; }

	int DeletedItemsId { get; set; }

	int DraftId { get; set; }

	int SentBoxId { get; set; }

	bool Delivery { get; set; }

	string RefrenceAccountId { get; set; }

	string Rstr4 { get; set; }

	string Rstr5 { get; set; }

	string SenderNumber { get; set; }

	int RefrencedId { get; set; }

	int AccountId { get; set; }
}
