using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebUploadFilePart : IUploadFilePart, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public long Order { get; set; }

	public Guid FileId { get; set; }

	public short RetryCount { get; set; }

	public bool IsSent { get; set; }

	public DateTime NextTry { get; set; }
}
