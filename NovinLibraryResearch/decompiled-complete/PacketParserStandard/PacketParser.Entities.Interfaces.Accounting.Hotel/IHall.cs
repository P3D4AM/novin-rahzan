using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IHall : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid? MasterGuid { get; set; }

	string Title { get; set; }
}
