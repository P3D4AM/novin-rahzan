using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IPrdSelectedDetList : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid MasterGuid { get; set; }

	string Description { get; set; }
}
