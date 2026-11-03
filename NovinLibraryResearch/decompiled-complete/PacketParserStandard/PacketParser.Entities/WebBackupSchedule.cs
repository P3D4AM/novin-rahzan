using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebBackupSchedule : IBackupSchedule, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; }

	public string Title { get; set; }

	public string ConnectionString { get; set; }

	public int Houre { get; set; }

	public DateTime CreateDate { get; set; }

	public string BackupPath { get; set; }

	public string CreatePath { get; }
}
