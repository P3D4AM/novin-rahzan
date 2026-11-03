using System;

namespace PacketParser.Entities;

public interface IBackupLog
{
	Guid Id { get; set; }

	Guid DPId { get; set; }

	Guid FileId { get; set; }

	DateTime LastBackup { get; set; }

	BackupState Status { get; set; }

	string Path { get; set; }

	DateTime LastStateChange { get; set; }

	string Message { get; set; }

	short RetryCount { get; set; }

	long TotalCrc { get; set; }

	string URL { get; set; }

	string FileName { get; set; }

	BackupType BackupType { get; set; }

	bool IsDelete { get; set; }

	string FileSize { get; }

	bool PathExists { get; }
}
