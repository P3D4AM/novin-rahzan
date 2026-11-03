using System;

namespace PacketParser.Entities.Interfaces.Attendance;

public interface IAttDepartment : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	Guid? AdminGuid { get; set; }

	DateTime CreatedDate { get; set; }
}
