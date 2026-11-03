using System;

namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public class AsteriskChangeQueueStatusEventArgs : System.EventArgs
{
	public string Queue { get; set; }

	public IssabelQueueState OldState { get; set; }

	public IssabelQueueState NewState { get; set; }

	public short MaxCountOrSeconds { get; set; }

	public EnIssabelErrorType IssabelErrorType { get; set; }
}
