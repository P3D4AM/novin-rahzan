using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core;

public interface ICompactCacheRepository<T> : ICompactRepository<T>, IDisposable where T : class, IHasGuid, IHasContext
{
	Task<ReturnedSaveFuncInfo> AddOrUpdateEntitiesAsync(T item, short tryCount, string tranName);

	Task<ReturnedSaveFuncInfo> AddRangeToEntitiesAsync(IEnumerable<T> items, short tryCount, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveFromEntitiesAsync(Guid item, short tryCount, CancellationToken token, string tranName);

	Task<T> GetEntityAsync(Guid guid, bool loadFromDatabaseIfNotCached, CancellationToken token, string tranName);
}
