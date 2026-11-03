using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IPrdSelectedListRepository : IRepository<PrdSelectedBusiness>, INoGuidRepository<PrdSelectedBusiness>, IDisposable
{
	Task<IEnumerable<PrdSelectedBusiness>> GetAllAsync(string title, string description, CancellationToken token, string tranName);
}
