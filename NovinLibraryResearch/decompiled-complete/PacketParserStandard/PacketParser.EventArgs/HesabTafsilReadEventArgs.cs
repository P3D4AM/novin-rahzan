using System;

namespace PacketParser.EventArgs;

public class HesabTafsilReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static HesabTafsilReadEventArgs FromString(Packet pack, string data)
	{
		HesabTafsilReadEventArgs hesabTafsilReadEventArgs = data.FromJson<HesabTafsilReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\HesabTree\\HesabTafsilReadEventArgs.cs", 15);
		hesabTafsilReadEventArgs.Part = pack.Part;
		hesabTafsilReadEventArgs.TotalParts = pack.TotalParts;
		hesabTafsilReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return hesabTafsilReadEventArgs;
	}
}
