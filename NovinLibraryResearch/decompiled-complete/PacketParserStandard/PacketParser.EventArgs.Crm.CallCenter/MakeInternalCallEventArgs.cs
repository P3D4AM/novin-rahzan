namespace PacketParser.EventArgs.Crm.CallCenter;

public class MakeInternalCallEventArgs : BaseCallEventArgs
{
	public string Caller { get; set; }

	public string CallTo { get; set; }

	public string Context { get; set; } = "default";


	public string Priority { get; set; } = "1";

}
