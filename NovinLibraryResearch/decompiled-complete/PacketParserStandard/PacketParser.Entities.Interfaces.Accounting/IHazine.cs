using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IHazine : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	decimal Account { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	string Code { get; set; }
}
