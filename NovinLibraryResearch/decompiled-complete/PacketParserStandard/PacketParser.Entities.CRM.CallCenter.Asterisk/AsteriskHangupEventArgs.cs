namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskHangupEventArgs : BaseAsteriskEventArgs
{
	public string ChannelStateDesc { get; set; }

	public int Cause { get; set; }

	public string Cause_txt { get; set; }

	public string ConnectedLineName { get; set; }

	public string CallerIdName { get; set; }
}
