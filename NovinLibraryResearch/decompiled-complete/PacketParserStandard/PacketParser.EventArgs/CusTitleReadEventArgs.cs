using System;

namespace PacketParser.EventArgs;

public class CusTitleReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static CusTitleReadEventArgs FromString(Packet pack, string data)
	{
		CusTitleReadEventArgs cusTitleReadEventArgs = data.FromJson<CusTitleReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Customer\\CusTitleReadEventArgs.cs", 15);
		cusTitleReadEventArgs.Part = pack.Part;
		cusTitleReadEventArgs.TotalParts = pack.TotalParts;
		cusTitleReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return cusTitleReadEventArgs;
	}
}
