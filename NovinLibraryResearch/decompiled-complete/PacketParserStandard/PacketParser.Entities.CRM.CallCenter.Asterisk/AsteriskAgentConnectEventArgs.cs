using System;

namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskAgentConnectEventArgs
{
	public string Channel { get; set; }

	public DateTime DateReceived { get; set; }

	public string Privilege { get; set; }

	public string Server { get; set; }

	public double Timestamp { get; set; }

	public string UniqueId { get; set; }

	public string LinkedId { get; set; }

	public string Queue { get; set; }

	public string Member { get; set; }

	public string MemberName { get; set; }

	public string DialedNumber { get; set; }

	public string CallerIdNumber { get; set; }
}
