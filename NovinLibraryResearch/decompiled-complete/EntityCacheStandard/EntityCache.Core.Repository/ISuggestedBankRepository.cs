using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface ISuggestedBankRepository : IRepository<SuggestedBankBusiness>, INoGuidRepository<SuggestedBankBusiness>, IDisposable
{
	Task<SuggestedBankBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
