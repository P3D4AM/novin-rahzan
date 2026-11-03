using System;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebMetaData : WebMetaData_Base, IMetaData, IMetaData_Base
{
	public Guid EntityGuid { get; set; }

	public EntityPart EntityType { get; set; }
}
