using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IHesabGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	int Code { get; set; }

	Guid TypeGuid { get; set; }
}
