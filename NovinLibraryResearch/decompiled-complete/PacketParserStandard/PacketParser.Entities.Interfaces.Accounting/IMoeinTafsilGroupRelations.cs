using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IMoeinTafsilGroupRelations
{
	Guid MoeinGuid { get; set; }

	Guid TafsilGroupGuid { get; set; }

	EnMoeinTafsilRelationLevel Level { get; set; }
}
