using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ITafsilSelected : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Code { get; set; }

	string Title { get; set; }

	short? Type { get; set; }

	Guid UGuid { get; set; }

	string Description { get; set; }
}
