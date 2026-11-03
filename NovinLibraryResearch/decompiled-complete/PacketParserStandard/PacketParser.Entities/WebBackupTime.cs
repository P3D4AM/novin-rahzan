using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebBackupTime : IBackupTime, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; }

	public Guid ParentId { get; set; }

	public DayOfWeek DayOfWeek { get; set; }

	public DateTime DateTime { get; set; }

	public TimeSpan Time { get; }
}
