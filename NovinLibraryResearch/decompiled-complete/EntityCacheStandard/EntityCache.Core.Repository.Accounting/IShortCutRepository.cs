using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IShortCutRepository : IRepository<ShortCutBusiness>, INoGuidRepository<ShortCutBusiness>, IDisposable
{
	Task<ShortCutBusiness> GetAsync(string formName, string name, CancellationToken token, string tranName);
}
