using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Entities.Uploader;

[Table("Acc_BackupLog")]
public class BackupLog : IBackupLog
{
	[Key]
	public Guid Id { get; set; }

	public Guid DPId { get; set; }

	public Guid FileId { get; set; }

	public DateTime LastBackup { get; set; }

	public BackupState Status { get; set; }

	[Required]
	public string Path { get; set; }

	public DateTime LastStateChange { get; set; } = DateTime.Now;


	[Required]
	public string Message { get; set; }

	public short RetryCount { get; set; }

	public long TotalCrc { get; set; }

	[Required]
	public string URL { get; set; } = "";


	public string? FileName { get; set; } = "";


	public BackupType BackupType { get; set; }

	public bool IsDelete { get; set; }

	public bool PathExists { get; }

	public string? FileSize { get; }
}
