using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Indexes;

namespace EntityCache.Core.Repository;

public interface IFullTextWordRepository : INoGuidRepository<FullTextWordBusiness>, IDisposable
{
	Task<Dictionary<string, int>> GetOrSaveAsync(List<string> words, CancellationToken token, string tranName);
}
