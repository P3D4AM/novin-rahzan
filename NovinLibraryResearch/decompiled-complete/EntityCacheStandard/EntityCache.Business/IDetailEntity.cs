using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public interface IDetailEntity<T> where T : class, IHasGuid, new()
{
}
