using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting;

public interface IPrdTechnicalRepository : IRepository<PrdTechnicalBusiness>, INoGuidRepository<PrdTechnicalBusiness>, IDisposable
{
	Task<PrdTechnicalBusiness> GetAsync(Guid guid, short technicalType, CancellationToken token, string tranName);

	Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(short technicalType, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(Guid prdGuid, bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveByProductGuidAsync(Guid prdGuid, CancellationToken token, string tranName);
}
