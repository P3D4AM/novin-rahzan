using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core;

public interface INoGuidRepository<T> : IDisposable where T : class, IHasContext
{
	WebContextHolder Context { get; }

	Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(T item, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<T> items, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<T>> GetAllAsync(CancellationToken token, string tranName);

	void ResetCache();
}
