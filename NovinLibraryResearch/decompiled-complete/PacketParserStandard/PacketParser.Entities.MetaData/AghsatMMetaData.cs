using System.Collections.Generic;
using PacketParser.Entities.Sms;

namespace PacketParser.Entities.MetaData;

public class AghsatMMetaData : Serializable<AghsatMMetaData>
{
	public List<SentSmsLog> SmsLogs { get; set; }
}
