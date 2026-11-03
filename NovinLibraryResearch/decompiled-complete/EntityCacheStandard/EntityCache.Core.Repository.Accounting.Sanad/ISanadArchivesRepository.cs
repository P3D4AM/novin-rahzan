using System;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting.Sanad;

public interface ISanadArchivesRepository : ISyncRepository<SanadArchivesBusiness>, IEditableDelivery, IRepository<SanadArchivesBusiness>, INoGuidRepository<SanadArchivesBusiness>, IDisposable
{
}
