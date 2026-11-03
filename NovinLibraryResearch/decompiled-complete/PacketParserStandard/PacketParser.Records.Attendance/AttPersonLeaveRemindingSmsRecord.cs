using System;
using PacketParser.Entities;

namespace PacketParser.Records.Attendance;

public record AttPersonLeaveRemindingSmsRecord
{
	public short DepId { get; set; }

	public EnPersonLeaveStatus Status { get; set; }

	public int Count { get; set; }

	public Guid? DepartmentGuid { get; set; }

	public Guid? AdminGuid { get; set; }
}
