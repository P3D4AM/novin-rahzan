namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskQueueCallerAbandonEventArgs : BaseAsteriskEventArgs
{
	public int HoldTime { get; set; }

	public int Position { get; set; }

	public string Queue { get; set; }
}
