using System;

namespace PacketParser.Entities.Interfaces.Global.Tasks;

public interface ITaskFileRelation
{
	Guid TaskGuid { get; set; }

	Guid FileGuid { get; set; }
}
