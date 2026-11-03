using System.ComponentModel;

namespace PacketParser.EventArgs;

public class ExecuteQueryResponseEventArgs : BaseEventArgs
{
	public string PacketRefrencedId { get; set; }

	public string JsonResult { get; set; }

	[DefaultValue(true)]
	public bool Result { get; set; } = true;


	public string ResultMessage { get; set; }

	public static ExecuteQueryResponseEventArgs FromString(Packet pack, string data)
	{
		ExecuteQueryResponseEventArgs executeQueryResponseEventArgs = data.FromJson<ExecuteQueryResponseEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ExecuteQuery\\ExecuteQueryResponseEventArgs.cs", 14);
		executeQueryResponseEventArgs.Part = pack.Part;
		executeQueryResponseEventArgs.TotalParts = pack.TotalParts;
		executeQueryResponseEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return executeQueryResponseEventArgs;
	}
}
