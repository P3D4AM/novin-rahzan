using System;

namespace Stimulsoft.Base.Server;

public class StiAccountNotificationEventArgs : EventArgs
{
	internal StiNotice Title { get; set; }

	public StiNotice Message { get; set; }

	internal StiAccountMessageIcon Icon { get; set; }

	internal StiAccountMessageButton Button { get; set; }

	internal object Build { get; set; }

	internal string ActionRenewUrl { get; set; }
}
