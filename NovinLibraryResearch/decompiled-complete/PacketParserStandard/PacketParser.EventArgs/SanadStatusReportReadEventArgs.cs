using System;

namespace PacketParser.EventArgs;

public class SanadStatusReportReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
