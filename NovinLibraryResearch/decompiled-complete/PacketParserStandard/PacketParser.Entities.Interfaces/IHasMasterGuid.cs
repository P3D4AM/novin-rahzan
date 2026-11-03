using System;

namespace PacketParser.Entities.Interfaces;

public interface IHasMasterGuid
{
	Guid MasterGuid { get; set; }
}
