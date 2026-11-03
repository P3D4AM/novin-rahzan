using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Uploader;

[Table("Acc_BackupSchedule")]
public class BackupSchedule : IBackupSchedule, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	[Required]
	public string Title { get; set; }

	[Required]
	public string ConnectionString { get; set; }

	public BackupType BackupType { get; set; }

	public int Houre { get; set; }

	public DateTime CreateDate { get; set; }

	[Required]
	public string BackupPath { get; set; }

	[Required]
	public string CreatePath { get; }
}
