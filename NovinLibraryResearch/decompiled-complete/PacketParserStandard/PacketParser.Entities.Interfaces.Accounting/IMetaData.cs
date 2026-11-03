using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IMetaData : IMetaData_Base
{
	Guid EntityGuid { get; set; }

	EntityPart EntityType { get; set; }
}
