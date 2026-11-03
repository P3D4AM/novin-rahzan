namespace PacketParser.EventArgs.Crm.CallCenter;

public class MakeOutGoingCallEventArgs : BaseCallEventArgs
{
	public string Caller { get; set; }

	public string CallTo { get; set; }

	public string Context { get; set; } = "from-internal";


	public string Priority { get; set; } = "1";

}
