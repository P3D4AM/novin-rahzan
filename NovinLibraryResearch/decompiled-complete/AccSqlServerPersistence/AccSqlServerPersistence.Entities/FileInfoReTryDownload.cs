using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_FileInfoReTryDownload")]
public class FileInfoReTryDownload : Serializable<IFileInfo>, IFileInfo, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public long Crc { get; set; }

	public long FileLen { get; set; }

	[Required]
	public string FileName { get; set; }

	public FileManagement_Parts Part { get; set; }

	[Required]
	public string SubDirectory { get; set; } = "";


	public bool Status { get; set; }

	public Guid EntityGuid { get; set; }

	[NotMapped]
	public string LocalFullPath { get; set; }

	[NotMapped]
	public string FullPath { get; set; }

	public DateTime Modified { get; set; }

	public string? Url { get; set; }

	public int ReTryCount { get; set; }

	public DateTime NextTryCount { get; set; }

	[NotMapped]
	public byte[] FileContent { get; set; }

	[NotMapped]
	public ENCompressedMode CompressMode { get; set; }
}
