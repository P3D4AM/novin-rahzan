using System;

namespace PacketParser.EventArgs;

public class CustomerReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static CustomerReadEventArgs FromString(Packet pack, string data)
	{
		CustomerReadEventArgs customerReadEventArgs = data.FromJson<CustomerReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Customer\\CustomerReadEventArgs.cs", 15);
		customerReadEventArgs.Part = pack.Part;
		customerReadEventArgs.TotalParts = pack.TotalParts;
		customerReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return customerReadEventArgs;
	}
}
