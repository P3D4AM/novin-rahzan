using System;
using EntityCache.Business;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface ISyncEntity : ISyncRepository<SyncEntityBusiness>, IEditableDelivery, IRepository<SyncEntityBusiness>, INoGuidRepository<SyncEntityBusiness>, IDisposable
{
}
