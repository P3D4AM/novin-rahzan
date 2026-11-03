using System;
using PacketParser.Entities;

namespace PacketParser.Payamak.Entities;

public interface ISmsNetAccount
{
	EnSmsSenderType Type { get; set; }

	decimal GsmCrecit { get; set; }

	DateTime? GsmUpdate { get; set; }

	short GsmMaxSent { get; set; }

	bool IsChecked { get; set; }

	short St { get; set; }

	string Description { get; set; }

	string Username { get; set; }

	int Id { get; set; }

	string Title { get; set; }

	string Password { get; set; }

	bool GSM_Connection { get; set; }

	bool GSM_On_Sendeng_Message { get; set; }

	bool GSMPDUMode { get; set; }

	int GSMNO { get; set; }

	string GSMSignal { get; set; }

	int GSMSendedSmsCount { get; set; }

	decimal? Account { get; }

	string Domain { get; set; }

	string FullDomain { get; set; }

	string SenderNumber { get; set; }
}
