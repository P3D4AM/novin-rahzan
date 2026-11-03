using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IBackupSchedule : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	string ConnectionString { get; set; }

	int Houre { get; set; }

	DateTime CreateDate { get; set; }

	string BackupPath { get; set; }

	string CreatePath { get; }
}
