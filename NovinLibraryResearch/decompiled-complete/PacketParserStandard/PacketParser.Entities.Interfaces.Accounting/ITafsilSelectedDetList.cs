using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ITafsilSelectedDetList : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short HType { get; set; }

	Guid MasterGuid { get; set; }

	string Description { get; set; }
}
