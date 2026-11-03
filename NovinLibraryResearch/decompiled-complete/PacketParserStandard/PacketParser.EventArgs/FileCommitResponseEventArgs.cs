using System;

namespace PacketParser.EventArgs;

public class FileCommitResponseEventArgs : BaseEventArgs
{
	public long Crc { get; set; }

	public bool Success { get; set; }

	public string Message { get; set; }

	public string FileName { get; set; }

	public string Url { get; set; }

	public Guid FileGuid { get; set; }

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
