using System;
using EntityCache.Business;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IVisitRepository : ISyncRepository<VisitBusiness>, IEditableDelivery, IRepository<VisitBusiness>, INoGuidRepository<VisitBusiness>, IDisposable
{
}
