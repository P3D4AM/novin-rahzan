using System;

namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskQueueCallerJoinEventArgs : AsteriskQueueCallerLeaveEnterEventArgs
{
	public DateTime JoinTime { get; set; } = DateTime.Now;

}
