using System;
using System.Collections.Generic;

namespace Stimulsoft.Base.Server;

public class StiAccountNotificationCollectionEventArgs : EventArgs
{
	public List<StiAccountNotificationEventArgs> Messages { get; set; }
}
