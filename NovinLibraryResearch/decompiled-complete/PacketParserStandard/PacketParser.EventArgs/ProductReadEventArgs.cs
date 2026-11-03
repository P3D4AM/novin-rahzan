using System;
using System.Collections.Generic;

namespace PacketParser.EventArgs;

public class ProductReadEventArgs : BaseEventArgs
{
	public DateTime? Modified { get; set; }

	public List<Guid> ProductGuids { get; set; }

	public List<Guid> GroupGuids { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
