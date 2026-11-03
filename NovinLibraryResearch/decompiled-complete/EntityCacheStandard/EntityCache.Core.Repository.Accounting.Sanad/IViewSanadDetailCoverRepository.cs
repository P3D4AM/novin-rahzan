using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Accounting.Sanad;

public interface IViewSanadDetailCoverRepository : IRepository<ViewSanadDetailCoverBusiness>, INoGuidRepository<ViewSanadDetailCoverBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveArticlesAsync(Guid asGuid, Document asType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveArticlesAsync(IEnumerable<ViewSanadDetailCoverBusiness> lst, CancellationToken token, string tranName);

	Task<IEnumerable<ViewSanadDetailCoverBusiness>> GetAllInvalidRelationsAsync(CancellationToken token, string tranName);
}
