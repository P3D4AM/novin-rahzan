using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IDecimalUpdatesRepository : IRepository<DecimalUpdatesBusiness>, INoGuidRepository<DecimalUpdatesBusiness>, IDisposable
{
	Task<IEnumerable<DecimalUpdatesBusiness>> GetAllByDecimalGuidAsync(Guid decimalGuid, bool? status, CancellationToken token, string tranName);
}
