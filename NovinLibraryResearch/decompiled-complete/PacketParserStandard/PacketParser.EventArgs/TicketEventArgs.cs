using System;

namespace PacketParser.EventArgs;

public class TicketEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static TicketEventArgs FromString(Packet pack, string data)
	{
		TicketEventArgs ticketEventArgs = data.FromJson<TicketEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Ticket\\TicketEventArgs.cs", 15);
		ticketEventArgs.Part = pack.Part;
		ticketEventArgs.TotalParts = pack.TotalParts;
		ticketEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return ticketEventArgs;
	}
}
