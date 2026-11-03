using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class CrmSettings : Serializable<CrmSettings>
{
	public int MaxAliveNotificationSeconds { get; set; } = 30000;

}
