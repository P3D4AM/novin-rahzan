using System;

namespace PacketParser.EventArgs;

public class TicketPartEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static TicketPartEventArgs FromString(Packet pack, string data)
	{
		TicketPartEventArgs ticketPartEventArgs = data.FromJson<TicketPartEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Ticket\\TicketPartEventArgs.cs", 15);
		ticketPartEventArgs.Part = pack.Part;
		ticketPartEventArgs.TotalParts = pack.TotalParts;
		ticketPartEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return ticketPartEventArgs;
	}
}
