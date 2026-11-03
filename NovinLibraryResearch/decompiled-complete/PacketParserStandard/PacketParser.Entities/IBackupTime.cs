using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IBackupTime : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ParentId { get; set; }

	DayOfWeek DayOfWeek { get; set; }

	DateTime DateTime { get; set; }

	TimeSpan Time { get; }
}
