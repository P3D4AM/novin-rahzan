using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IUploadFile : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid Id { get; set; }

	string Name { get; set; }

	long Crc { get; set; }

	FileManagement_Parts FileManagementParts { get; set; }

	int MinChunk { get; set; }

	int MaxChunk { get; set; }

	string UrlPath { get; set; }

	int PartCount { get; set; }

	int PartSent { get; set; }

	string SubDirectory { get; set; }

	short RetryCount { get; set; }

	DateTime InsertTime { get; set; }

	Guid EntityGuid { get; set; }
}
