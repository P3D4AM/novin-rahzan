using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttPersonLeave : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid PersonGuid { get; set; }

	DateTime RequestDate { get; set; }

	string Reason { get; set; }

	DateTime DateFrom { get; set; }

	DateTime DateTo { get; set; }

	EnPersonLeaveType Type { get; set; }

	EnPersonLeaveStatus LeaveStatus { get; set; }

	EnPersonLeaveKind Kind { get; set; }

	string ManagerMessage { get; set; }

	DateTime? ManagerDate { get; set; }

	EnPersonLeaveQuiddity Quiddity { get; set; }
}
