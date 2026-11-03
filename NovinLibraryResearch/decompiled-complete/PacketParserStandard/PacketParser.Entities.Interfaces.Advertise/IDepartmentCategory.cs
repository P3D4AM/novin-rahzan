using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IDepartmentCategory : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid DepartmentGuid { get; set; }

	string FirstLevel { get; set; }

	string SecondLevel { get; set; }

	string ThirdLevel { get; set; }
}
