using System;

namespace PacketParser.EventArgs.Crm.CallCenter;

public class QueueAbandonEventArgs : BaseCallEventArgs
{
	public string QueeName { get; set; }

	public string QueeNumber { get; set; }

	public string CustomerNumber { get; set; }

	public DateTime Date { get; set; }

	public int Position { get; set; }

	public int HoldTime { get; set; }
}
