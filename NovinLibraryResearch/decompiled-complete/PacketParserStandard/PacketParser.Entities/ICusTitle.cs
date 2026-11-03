using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ICusTitle : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	ServerStatusForClient ServerStatus { get; set; }
}
