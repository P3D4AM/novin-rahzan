using System.Collections.Generic;
using PacketParser.Entities.Sms;

namespace PacketParser.Entities.MetaData;

public class ReceptionChequeMetaData : Serializable<ReceptionChequeMetaData>
{
	public List<SentSmsLog> SmsLogs { get; set; }

	public EnChequeType ChequeType { get; set; }
}
