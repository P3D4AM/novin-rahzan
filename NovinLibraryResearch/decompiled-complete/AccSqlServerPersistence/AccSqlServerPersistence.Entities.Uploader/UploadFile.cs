using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Uploader;

[Table("Acc_UploadFile")]
public class UploadFile : IUploadFile, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Id { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[Required]
	public string Name { get; set; }

	public long Crc { get; set; }

	public FileManagement_Parts FileManagementParts { get; set; }

	public int MinChunk { get; set; }

	public int MaxChunk { get; set; }

	[Required]
	public string UrlPath { get; set; }

	public int PartCount { get; set; }

	public int PartSent { get; set; }

	[Required]
	public string SubDirectory { get; set; }

	public short RetryCount { get; set; }

	public DateTime InsertTime { get; set; } = DateTime.Now;


	public Guid EntityGuid { get; set; }
}
