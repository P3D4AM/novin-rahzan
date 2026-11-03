using System;

namespace PacketParser.EventArgs;

public class HyperLinkReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
