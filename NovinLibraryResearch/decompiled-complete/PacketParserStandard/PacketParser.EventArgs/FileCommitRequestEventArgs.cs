using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class FileCommitRequestEventArgs : BaseEventArgs
{
	public List<string> PartNames { get; set; }

	public string FileName { get; set; }

	public FileManagement_Parts FilePart { get; set; }

	public long Crc { get; set; }

	public string SubDirectory { get; set; }

	public Guid FileGuid { get; set; }

	public Guid EntityGuid { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
