namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskQueueCallerLeaveEnterEventArgs : BaseAsteriskEventArgs
{
	public string Queue { get; set; }

	public string Position { get; set; }

	public int Count { get; set; }
}
