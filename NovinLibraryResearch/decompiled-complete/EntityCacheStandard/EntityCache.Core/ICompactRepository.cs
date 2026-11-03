using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core;

public interface ICompactRepository<T> : IDisposable where T : class, IHasGuid, IHasContext
{
	WebContextHolder Context { get; }

	Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName);

	Task<List<T>> GetAllAsync(bool? status, CancellationToken token, string tranName);

	Task<T> GetAsync(Guid guid, CancellationToken token, string tranName);

	T Get(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeFromEntitiesAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName);
}
