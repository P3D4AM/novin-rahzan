using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface ICardTargetRelation
{
	Guid TargetGuid { get; set; }

	Guid CardGuid { get; set; }
}
