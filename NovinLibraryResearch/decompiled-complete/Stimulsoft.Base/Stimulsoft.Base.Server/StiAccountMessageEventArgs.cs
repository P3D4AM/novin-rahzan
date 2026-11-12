using System;

namespace Stimulsoft.Base.Server;

public class StiAccountMessageEventArgs : EventArgs
{
	public string Title { get; set; }

	public string Message { get; set; }

	internal StiAccountMessageIcon Icon { get; set; }

	internal StiAccountMessageButton Button { get; set; }
}
