using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface INaghzRepository : IRepository<NaghzBusiness>, INoGuidRepository<NaghzBusiness>, IDisposable
{
	Task<NaghzBusiness> GetAsync(CancellationToken token, string tranName);
}
