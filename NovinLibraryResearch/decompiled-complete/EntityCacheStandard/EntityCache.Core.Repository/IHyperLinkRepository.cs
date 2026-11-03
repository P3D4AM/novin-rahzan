using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface IHyperLinkRepository : IRepository<HyperLinkBusiness>, INoGuidRepository<HyperLinkBusiness>, IDisposable
{
	Task<IEnumerable<HyperLinkBusiness>> GetByCategoryAsync(string category, int top, bool? status, CancellationToken token, string tranName);
}
