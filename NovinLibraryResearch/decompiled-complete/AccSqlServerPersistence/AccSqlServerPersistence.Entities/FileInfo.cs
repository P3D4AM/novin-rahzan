using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_FileInfo")]
public class FileInfo : Serializable<IFileInfo>, IFileInfo, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
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

	public byte[]? FileContent { get; set; }

	public ENCompressedMode CompressMode { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public string? MetaData { get; set; }
}
