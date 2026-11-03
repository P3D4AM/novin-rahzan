using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Interfaces;

public interface IOrderV5 : IHasContext, IHasJustGuid
{
	Document Type { get; set; }
}
