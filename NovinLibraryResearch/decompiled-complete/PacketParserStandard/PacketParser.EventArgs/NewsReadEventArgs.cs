using System;

namespace PacketParser.EventArgs;

public class NewsReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
