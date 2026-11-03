using System;

namespace PacketParser.EventArgs;

public class DepotReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static DepotReadEventArgs FromString(Packet pack, string data)
	{
		DepotReadEventArgs depotReadEventArgs = data.FromJson<DepotReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Depot\\DepotReadEventArgs.cs", 15);
		depotReadEventArgs.Part = pack.Part;
		depotReadEventArgs.TotalParts = pack.TotalParts;
		depotReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return depotReadEventArgs;
	}
}
