using System;

namespace ClientHub;

public class ChangeSendDelayEventArgs : EventArgs
{
	public long MilisecondsDelayBetweenSend_old { get; set; }

	public long MilisecondsDelayBetweenSend_new { get; set; }
}
