using System;

namespace PacketParser.EventArgs;

public class HesabKolReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static HesabKolReadEventArgs FromString(Packet pack, string data)
	{
		HesabKolReadEventArgs hesabKolReadEventArgs = data.FromJson<HesabKolReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\HesabTree\\HesabKolReadEventArgs.cs", 15);
		hesabKolReadEventArgs.Part = pack.Part;
		hesabKolReadEventArgs.TotalParts = pack.TotalParts;
		hesabKolReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return hesabKolReadEventArgs;
	}
}
