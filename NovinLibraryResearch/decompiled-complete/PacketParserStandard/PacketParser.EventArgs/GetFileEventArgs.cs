using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class GetFileEventArgs : BaseEventArgs
{
	public IEnumerable<WebFileInfo> FileInfos { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
