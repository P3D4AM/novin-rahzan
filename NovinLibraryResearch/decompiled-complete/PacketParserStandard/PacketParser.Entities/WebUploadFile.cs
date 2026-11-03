using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebUploadFile : IUploadFile, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid Id { get; set; }

	public string Name { get; set; }

	public long Crc { get; set; }

	public FileManagement_Parts FileManagementParts { get; set; }

	public int MinChunk { get; set; }

	public int MaxChunk { get; set; }

	public string UrlPath { get; set; }

	public int PartCount { get; set; }

	public int PartSent { get; set; }

	public string SubDirectory { get; set; }

	public short RetryCount { get; set; }

	public DateTime InsertTime { get; set; }

	public string DateSh { get; }

	public Guid EntityGuid { get; set; }
}
