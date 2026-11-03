using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IDecimalRepository : IRepository<DecimalBusiness>, INoGuidRepository<DecimalBusiness>, IDisposable
{
	Task<DecimalBusiness> GetAsync(string name, string country, CancellationToken token, string tranName);

	Task<DecimalBusiness> GetAsync(string slug, CancellationToken token, string tranName);
}
