using System;

namespace PacketParser.Entities.Interfaces;

public interface IVisitorArea : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short Level { get; set; }

	string Name { get; set; }

	Guid? ParentGuid { get; set; }

	string Description { get; set; }

	long Code { get; set; }

	long FullCode { get; set; }
}
