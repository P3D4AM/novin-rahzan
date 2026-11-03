using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TozieList;

namespace EntityCache.Core.Repository.Accounting.TozieList;

public interface ITozieListRepository : IRepository<TozieListBusiness>, INoGuidRepository<TozieListBusiness>, IDisposable
{
	Task<IEnumerable<TozieListRepostBusiness>> GetAllReportAsync(CancellationToken token, string tranName);

	Task<int> GetNextNumberAsync(CancellationToken token, string tranName);
}
