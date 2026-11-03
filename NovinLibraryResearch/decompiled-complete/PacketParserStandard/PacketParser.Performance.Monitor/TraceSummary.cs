namespace PacketParser.Performance.Monitor;

public class TraceSummary
{
	public string ClassName { get; set; }

	public string FuntionName { get; set; }

	public ulong Count { get; set; }

	public ulong ElapsedSum { get; set; }
}
