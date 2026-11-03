using System;

namespace PacketParser.EventArgs.Crm.CallCenter;

public class QueueAssingedToAgentEventArgs : BaseCallEventArgs
{
	public string AgentName { get; set; }

	public string AgentNumber { get; set; }

	public string QueeName { get; set; }

	public string QueeNumber { get; set; }

	public string CustomerNumber { get; set; }

	public DateTime Date { get; set; }

	public string Position { get; set; }

	public int Count { get; set; }
}
