namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskNewStateEventArgs : BaseAsteriskEventArgs
{
	public string ChannelState { get; set; }

	public string ChannelStateDesc { get; set; }

	public string CallerId { get; set; }

	public string CallerIdNumber { get; set; }

	public string CallerIdName { get; set; }

	public string State { get; set; }

	public string AccountCode { get; set; }

	public string ConnectedLineNumber { get; set; }

	public string ConnectedLineName { get; set; }

	public bool IsIncoming { get; set; }
}
