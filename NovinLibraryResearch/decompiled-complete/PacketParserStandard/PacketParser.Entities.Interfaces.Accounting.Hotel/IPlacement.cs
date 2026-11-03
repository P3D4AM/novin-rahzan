using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IPlacement : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid? MasterGuid { get; set; }

	Guid? GroupGuid { get; set; }

	int ItemCount { get; set; }

	decimal Price { get; set; }

	decimal ExtraPrice { get; set; }

	string Title { get; set; }

	string Description { get; set; }
}
