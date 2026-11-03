using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IRoomKardex : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid RoomGuid { get; set; }

	int OldStatus { get; set; }

	int NewStatus { get; set; }

	DateTime DateM { get; set; }

	Guid CashiarGuid { get; set; }

	int Ordinal { get; set; }

	string Discription { get; set; }

	Guid AsGuid { get; set; }

	EnAsTypy AsType { get; set; }
}
