using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IUploadFilePart : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long Order { get; set; }

	Guid FileId { get; set; }

	short RetryCount { get; set; }

	bool IsSent { get; set; }

	DateTime NextTry { get; set; }
}
