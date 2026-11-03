using System;

namespace PacketParser.EventArgs;

public class HesabMoeinReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static HesabMoeinReadEventArgs FromString(Packet pack, string data)
	{
		HesabMoeinReadEventArgs hesabMoeinReadEventArgs = data.FromJson<HesabMoeinReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\HesabTree\\HesabMoeinReadEventArgs.cs", 15);
		hesabMoeinReadEventArgs.Part = pack.Part;
		hesabMoeinReadEventArgs.TotalParts = pack.TotalParts;
		hesabMoeinReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return hesabMoeinReadEventArgs;
	}
}
