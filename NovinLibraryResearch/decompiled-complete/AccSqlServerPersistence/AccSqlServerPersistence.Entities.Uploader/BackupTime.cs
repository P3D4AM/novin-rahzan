using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Uploader;

[Table("Acc_BackupTime")]
public class BackupTime : IBackupTime, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Guid ParentId { get; set; }

	public DayOfWeek DayOfWeek { get; set; }

	public DateTime DateTime { get; set; }

	[NotMapped]
	public TimeSpan Time { get; }
}
