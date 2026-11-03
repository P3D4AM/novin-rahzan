using System;

namespace PacketParser.EventArgs;

public class GpsReadEventArgs : BaseEventArgs
{
	public int RequestedUId { get; set; }

	public DateTime Date1 { get; set; }

	public DateTime Date2 { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
