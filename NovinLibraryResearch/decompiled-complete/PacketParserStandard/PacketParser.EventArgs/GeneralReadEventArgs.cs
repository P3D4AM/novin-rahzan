using System;

namespace PacketParser.EventArgs;

public class GeneralReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static GeneralReadEventArgs FromString(Packet pack, string data)
	{
		GeneralReadEventArgs generalReadEventArgs = data.FromJson<GeneralReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\GeneralReadEventArgs.cs", 15);
		generalReadEventArgs.Part = pack.Part;
		generalReadEventArgs.TotalParts = pack.TotalParts;
		generalReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return generalReadEventArgs;
	}
}
