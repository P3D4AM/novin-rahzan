using System;

namespace PacketParser.EventArgs;

public class TicketDetEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static TicketDetEventArgs FromString(Packet pack, string data)
	{
		TicketDetEventArgs ticketDetEventArgs = data.FromJson<TicketDetEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Ticket\\TicketDetEventArgs.cs", 15);
		ticketDetEventArgs.Part = pack.Part;
		ticketDetEventArgs.TotalParts = pack.TotalParts;
		ticketDetEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return ticketDetEventArgs;
	}
}
