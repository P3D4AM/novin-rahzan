using System;

namespace PacketParser.Connection.LowLevel;

public class ChangeSendDelayEventArgs : System.EventArgs
{
	public long MilisecondsDelayBetweenSend_old { get; set; }

	public long MilisecondsDelayBetweenSend_new { get; set; }
}
