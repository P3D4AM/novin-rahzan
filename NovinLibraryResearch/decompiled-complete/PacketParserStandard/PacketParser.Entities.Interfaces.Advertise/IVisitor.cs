using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IVisitor : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short MasterCount { get; set; }

	short SlaveCount { get; set; }

	bool IsAgent { get; set; }

	string AdvReplacement { get; set; }

	string FullName { get; set; }

	string Enable_Status { get; set; }

	string Phone { get; set; }

	Guid DepartmentGuid { get; set; }
}
