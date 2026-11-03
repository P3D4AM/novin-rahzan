using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IDepot : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Description { get; set; }

	ReturnedState NegativeAmount { get; set; }

	string Name { get; set; }
}
