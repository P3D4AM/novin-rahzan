using System;
using EntityCache.Business.Accounting.Gift;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting.Gift;

public interface IGiftRepository : ISyncRepository<GiftBusiness>, IEditableDelivery, IRepository<GiftBusiness>, INoGuidRepository<GiftBusiness>, IDisposable
{
}
