using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IVistorAreaRepository : ISyncRepository<VisitorAreaBusiness>, IEditableDelivery, IRepository<VisitorAreaBusiness>, INoGuidRepository<VisitorAreaBusiness>, IDisposable
{
	Task<VisitorAreaBusiness> GetAsync(long code, Guid? parentGuid, CancellationToken token, string tranName);

	Task<IEnumerable<VisitorAreaBusiness>> GetAllAsync(Guid? parentGuid, bool? status, CancellationToken token, string tranName);

	Task<long> GetNextCodeAsync(short? level, CancellationToken token, string tranName);

	Task<IEnumerable<VisitorAreaBusiness>> GetChildrenAsync(Guid parentGuid, CancellationToken token, string tranName);
}
