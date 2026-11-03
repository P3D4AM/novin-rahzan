using System;

namespace PacketParser.Entities;

public class WebBackupLog : IBackupLog
{
	public Guid Id { get; set; }

	public Guid DPId { get; set; }

	public Guid FileId { get; set; }

	public DateTime LastBackup { get; set; }

	public BackupState Status { get; set; }

	public string Path { get; set; }

	public DateTime LastStateChange { get; set; }

	public string Message { get; set; }

	public short RetryCount { get; set; }

	public long TotalCrc { get; set; }

	public string URL { get; set; }

	public string FileName { get; set; }

	public BackupType BackupType { get; set; }

	public bool IsDelete { get; set; }

	public string DateSh { get; }

	public string FileSize { get; }

	public bool PathExists { get; }
}
