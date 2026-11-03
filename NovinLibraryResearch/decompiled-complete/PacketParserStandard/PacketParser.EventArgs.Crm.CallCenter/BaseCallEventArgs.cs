namespace PacketParser.EventArgs.Crm.CallCenter;

public class BaseCallEventArgs
{
	public string Uuid { get; set; }

	public string CallCenterIp { get; set; }

	public int CallCenterPort { get; set; }
}
