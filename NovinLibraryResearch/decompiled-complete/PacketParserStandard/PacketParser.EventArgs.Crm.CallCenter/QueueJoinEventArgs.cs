using System;

namespace PacketParser.EventArgs.Crm.CallCenter;

public class QueueJoinEventArgs : BaseCallEventArgs
{
	public string QueeName { get; set; }

	public string QueeNumber { get; set; }

	public string CustomerNumber { get; set; }

	public DateTime Date { get; set; }

	public int Position { get; set; }

	public int Count { get; set; }
}
