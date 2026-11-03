using System;

namespace PacketParser.Entities.Interfaces.Accounting.Selection;

public interface IProductGroupRelation
{
	Guid ProductGuid { get; set; }

	Guid GroupGuid { get; set; }

	short Ordinal { get; set; }
}
