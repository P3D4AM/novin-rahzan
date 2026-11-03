using System;

namespace PacketParser.EventArgs;

public class CusGroupReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static CusGroupReadEventArgs FromString(Packet pack, string data)
	{
		CusGroupReadEventArgs cusGroupReadEventArgs = data.FromJson<CusGroupReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Customer\\CusGroupReadEventArgs.cs", 15);
		cusGroupReadEventArgs.Part = pack.Part;
		cusGroupReadEventArgs.TotalParts = pack.TotalParts;
		cusGroupReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return cusGroupReadEventArgs;
	}
}
