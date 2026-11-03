using System;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface ISanadStatusReportUpdatesRepository : ISyncRepository<SanadStatusReportUpdatesBusiness>, IEditableDelivery, IRepository<SanadStatusReportUpdatesBusiness>, INoGuidRepository<SanadStatusReportUpdatesBusiness>, IDisposable
{
}
