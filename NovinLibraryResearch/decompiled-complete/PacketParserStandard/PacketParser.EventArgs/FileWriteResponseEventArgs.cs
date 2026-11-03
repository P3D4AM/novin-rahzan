using System.Collections.Generic;

namespace PacketParser.EventArgs;

public class FileWriteResponseEventArgs : BaseEventArgs
{
	public List<string> PartNames { get; set; }

	public List<bool> Successes { get; set; }

	public List<string> Messages { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
