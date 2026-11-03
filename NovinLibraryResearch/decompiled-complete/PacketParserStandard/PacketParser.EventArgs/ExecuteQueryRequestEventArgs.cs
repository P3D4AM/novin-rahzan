using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class ExecuteQueryRequestEventArgs : BaseEventArgs
{
	public string JsonRequest { get; set; }

	public PacketType Type { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static ExecuteQueryRequestEventArgs FromString(Packet pack, string data)
	{
		ExecuteQueryRequestEventArgs executeQueryRequestEventArgs = data.FromJson<ExecuteQueryRequestEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ExecuteQuery\\ExecuteQueryRequestEventArgs.cs", 13);
		executeQueryRequestEventArgs.Part = pack.Part;
		executeQueryRequestEventArgs.TotalParts = pack.TotalParts;
		executeQueryRequestEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return executeQueryRequestEventArgs;
	}
}
