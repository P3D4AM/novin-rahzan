using System;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebMoeinTafsilGroupRelation : IMoeinTafsilGroupRelations
{
	public Guid MoeinGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public EnMoeinTafsilRelationLevel Level { get; set; }
}
