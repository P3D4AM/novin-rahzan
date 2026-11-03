using System;
using PacketParser.Entities;
using PacketParser.Payamak.Interfaces.Entities;

namespace PacketParser.Payamak.Entities;

public class BaseSmsOutBox : BaseSms, ISmsOutBox, IBaseSms
{
	public DateTime SchadulingM { get; set; } = DateTime.Now;


	public string SchadulingSh => SchadulingM.MiladiToShamsi();

	public SMSSchadulingType SchadulingType { get; set; }

	public short Priority { get; set; }

	public int DeletedItemsId { get; set; }

	public int DraftId { get; set; }

	public int SentBoxId { get; set; }

	public bool Delivery { get; set; }

	public string RefrenceAccountId { get; set; }

	public string Rstr4 { get; set; }

	public string Rstr5 { get; set; }

	public string SenderNumber { get; set; }

	public int RefrencedId { get; set; }

	public int AccountId { get; set; }

	public int TryCount { get; set; }

	public DateTime NextTry { get; set; }
}
