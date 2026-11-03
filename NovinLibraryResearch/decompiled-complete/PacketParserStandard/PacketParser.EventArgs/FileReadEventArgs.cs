using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class FileReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public ENSource EnSource { get; set; }

	public short DepId { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
